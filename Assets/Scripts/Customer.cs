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
    private Animator animator;
    private TableController table;


    void start()
    {
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
        Debug.Log("Customer Awake - Agent and UIManager initialized");
        
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        Debug.Log($"Customer Update - Distance to target: {distanceToTarget}, Stopping distance: {agent.stoppingDistance}");
        float speed = agent.velocity.magnitude;   
        animator.SetFloat("Speed", speed); 

        if (distanceToTarget <= 1f)
        {
            Debug.Log("Customer reached destination point");
            animator.SetFloat("Speed", 0f);
            agent.isStopped = true;
            agent.ResetPath();
            StartCoroutine(TurnCharacterTowardsTable(table.transform.position));
            
                
        }
        return;
    }
    private IEnumerator TurnCharacterTowardsTable(Vector3 tablePos)
    {
        yield return new WaitForSeconds(0.2f); // küçük bir gecikme
        Vector3 direction = (tablePos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }


    public void NewOrder(Foods food, GameObject targetPoint, TableController table_)
    {
        Debug.Log($"NewOrder called with food: {food.foodName}, table: {(table != null ? table.name : "null")}");
        table = table_;
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