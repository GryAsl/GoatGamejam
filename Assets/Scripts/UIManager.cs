using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIManager : MonoBehaviour
{
    public GameObject[] orderList;
    public Foods[] foodList;

    public TextMeshProUGUI newOrderText;
    public Image newOrderImage;
    public float newOrderSpeedMultiplier = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewOrder()
    {
        foreach(var orderPanel in orderList)
        {
            if (!orderPanel.GetComponent<OrderStatus>().on)
            {
                int i = Random.Range(0, foodList.Length);
                orderPanel.GetComponent<OrderStatus>().NewOrder(foodList[i]);
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

        yield return new WaitForSecondsRealtime(1f);

        imColor = im.color;
        while (text.alpha >= 0f)
        {
            text.alpha -= Time.deltaTime * newOrderSpeedMultiplier;
            imColor.a -= Time.deltaTime * newOrderSpeedMultiplier;
            im.color = imColor;
            yield return null;
        }
    }
}
