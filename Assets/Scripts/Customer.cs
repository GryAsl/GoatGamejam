using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    
    public Foods currentFood;

    public UIManager uiMan;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    void Update()
    {
        
    }

    public void NewOrder(Foods food, GameObject targetGO)
    {
        currentFood = food;
        uiMan.NewOrder(currentFood);
        Debug.Log(targetGO);

        agent.destination = targetGO.transform.position;
    }


}
