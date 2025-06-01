using TMPro;
using UnityEngine;

public class OrderStatus : MonoBehaviour
{
    public bool on;
    public float currentTimer;
    public Foods food;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ingredientsText;
    public TextMeshProUGUI timerText;

    public CanvasGroup cg;
    public GameManager gm;

    Color startColor;
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            currentTimer += Time.deltaTime * gm.timerMultiplier;
            timerText.text = "Timer: " + currentTimer.ToString("F1");
            if(currentTimer >= food.maxTime)
            {
                gm.ENDGAME();
            }
            if(currentTimer >= food.maxTime * .5f && currentTimer < food.maxTime){
                timerText.color = Color.Lerp(startColor, Color.red, (currentTimer / (food.maxTime / 2f)) - 1);
                Debug.Log((currentTimer / (food.maxTime / 2f)) - 1);
            }
        }
    }

    public void NewOrder(Foods food_)
    {
        on = true;
        food = food_;
        ingredientsText.text = "Ingredients: " + food.Ingredients;
        nameText.text =  food.foodName;
        startColor = timerText.color;

    }
}
