using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIManager : MonoBehaviour
{
    public GameObject[] orderList;

    public TextMeshProUGUI newOrderText;
    public Image newOrderImage;
    public float newOrderSpeedMultiplier = 2f;
    public float UIspeedMultiplier = 2f;

    public GameObject buildingPanel;
    public GameObject mainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewOrder(Foods food)
    {
        foreach(var orderPanel in orderList)
        {
            if (!orderPanel.GetComponent<OrderStatus>().on)
            {
                orderPanel.GetComponent<OrderStatus>().NewOrder(food);
                StartCoroutine(NewOrder(orderPanel.GetComponent<OrderStatus>(), newOrderText, newOrderImage));
                break;

            }
             
        }
    }

    IEnumerator TurnOnText(TextMeshProUGUI text)
    {
        while(text.alpha <= 1f)
        {
            text.alpha += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator NewOrder(OrderStatus orderStatus, TextMeshProUGUI text, Image im)
    {
        Color imColor = im.color;
        while (text.alpha <= 1f)
        {
            orderStatus.cg.alpha += Time.deltaTime * newOrderSpeedMultiplier;
            text.alpha += Time.deltaTime * newOrderSpeedMultiplier;
            imColor.a += Time.deltaTime * newOrderSpeedMultiplier;
            im.color = imColor;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(.5f);

        imColor = im.color;
        while (text.alpha >= 0f)
        {
            text.alpha -= Time.deltaTime * newOrderSpeedMultiplier;
            imColor.a -= Time.deltaTime * newOrderSpeedMultiplier;
            im.color = imColor;
            yield return null;
        }
    }

    public IEnumerator TurnOnPanel(GameObject GO)
    {
        CanvasGroup cg = GO.GetComponent<CanvasGroup>();
        cg.interactable = true;
        float t = cg.alpha;
        while (t <= 1f)
        {
            t += Time.deltaTime * UIspeedMultiplier;
            cg.alpha = t;
            yield return null;
        }
    }

    public IEnumerator TurnOffPanel(GameObject GO)
    {
        CanvasGroup cg = GO.GetComponent<CanvasGroup>();
        cg.interactable = false;
        float t = cg.alpha;
        while (t >= 0f)
        {
            t -= Time.deltaTime * UIspeedMultiplier;
            cg.alpha = t;
            yield return null;
        }
    }




}
