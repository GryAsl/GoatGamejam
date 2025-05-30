using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Foods currentFood;
    public UIManager uiMan;
    private NavMeshAgent agent;
    private TableController currentTable;
    private Vector3 targetPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
        Debug.Log("Customer Awake - Agent and UIManager initialized");
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        Debug.Log($"Customer Update - Distance to target: {distanceToTarget}, Stopping distance: {agent.stoppingDistance}");
        
        if (distanceToTarget <= agent.stoppingDistance)
        {
            Debug.Log("Customer reached destination point");
            enabled = false; // Disable the customer after reaching the table
        }
    }

    public void NewOrder(Foods food, GameObject targetPoint, TableController table)
    {
        Debug.Log($"NewOrder called with food: {food.foodName}, table: {(table != null ? table.name : "null")}");
        
        currentFood = food;
        uiMan.NewOrder(currentFood);
        
        currentTable = table;
        if (currentTable == null)
        {
            Debug.LogError("Table reference is null!");
            return;
        }

        targetPosition = targetPoint.transform.position;
        Debug.Log($"Setting destination to: {targetPosition}");
        agent.destination = targetPosition;
    }


}
