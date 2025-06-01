using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Foods currentFood;
    public UIManager uiMan;
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private Animator animator;
    private TableController table;
    private TableController currentTable;
    
    [SerializeField] private Transform doorPosition; // Position of the exit door
    private bool isEating = false;
    private bool isLeaving = false;
    private float eatingCountdown = 5f;

    GameManager gameManager;


    void Start()
    {
        // If doorPosition is not assigned in the inspector, find it by tag
        if (doorPosition == null)
        {
            GameObject door = GameObject.FindGameObjectWithTag("Door");
            if (door != null)
            {
                doorPosition = door.transform;
            }
            else
            {
                Debug.LogError("Door not found! Make sure there's a GameObject with 'Door' tag in the scene.");
            }
        }
    }
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        uiMan = GameObject.Find("GameManager").GetComponent<UIManager>();
        Debug.Log("Customer Awake - Agent and UIManager initialized");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    

    void Update()
    {
        if (isEating)
        {
            // Count down while eating
            eatingCountdown -= Time.deltaTime;
            
            // When countdown reaches zero, walk to the door
            if (eatingCountdown <= 0f && !isLeaving)
            {
                StartCoroutine(LeaveRestaurant());
            }
            return;
        }
        
        if (isLeaving)
        {
            // Check if reached the door
            float distanceToDoor = Vector3.Distance(transform.position, doorPosition.position);
            if (distanceToDoor <= 2f)
            {
                Destroy(gameObject);
                return;
            }
        }
        
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        //Debug.Log($"Customer Update - Distance to target: {distanceToTarget}, Stopping distance: {agent.stoppingDistance}");
        float speed = agent.velocity.magnitude;   
        animator.SetFloat("Speed", speed); 

        if (distanceToTarget <= 2f && !isEating && !isLeaving)
        {
            //Debug.Log("Customer reached destination point");
            animator.SetFloat("Speed", 0f);
            agent.isStopped = true;
            agent.ResetPath();
            StartCoroutine(TurnCharacterTowardsTable(table.transform.position));
        }
    }
    
    // Called when a plate is delivered to the table
    public void OnPlateDelivered(Plate plate)
    {
        //Debug.Log("Plate delivered to customer. Starting eating countdown.");
        isEating = true;
        eatingCountdown = 5f;
        
        // Tell the table that this customer will be leaving
        if (plate != null)
        {
            StartCoroutine(DestroyPlateAfterDelay(plate));
        }
    }
    
    private IEnumerator DestroyPlateAfterDelay(Plate plate)
    {
        yield return new WaitForSeconds(5f);
        
        if (plate != null)
        {
            Vector3 platePosition = plate.transform.position;
            Quaternion plateRotation = plate.transform.rotation;
            
            // Spawn dirty plate at the same position
            if (currentTable != null && currentTable.dirtyPlatePrefab != null)
            {
                GameObject dirtyPlate = Instantiate(currentTable.dirtyPlatePrefab, platePosition, plateRotation);
                dirtyPlate.transform.SetParent(currentTable.transform);
                
                // Increment score
                if (gameManager != null)
                {
                    gameManager.score += 1;
                    if (gameManager.score >= 5)
                        gameManager.Level1Passed();
                }
            }
            
            // Destroy the original plate
            Destroy(plate.gameObject);
        }
    }
    
    private IEnumerator LeaveRestaurant()
    {
        isLeaving = true;
        isEating = false;
        
        // Resume navigation
        agent.isStopped = false;
        
        // Set destination to the door
        if (doorPosition != null)
        {
            agent.SetDestination(doorPosition.position);
            Debug.Log("Customer is leaving the restaurant.");
        }
        else
        {
            Debug.LogError("Door position is not set!");
        }

        LeaveTable();
        yield return null;
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
        
        // Tell the table which customer is associated with it
        currentTable.SetCustomer(this);

        targetPosition = targetPoint.transform.position;
        Debug.Log($"Setting destination to: {targetPosition}");
        agent.destination = targetPosition;
    }
    
    // Masaya referansın varsa:
    public void LeaveTable()
    {
        if (table != null)
        {
            // Masa üzerinden CustomerManager'ın PointData'sını bul
            CustomerManager manager = FindObjectOfType<CustomerManager>();
            foreach (var pd in manager.pointData)
            {
                if (pd.table == table)
                {
                    pd.isEmpty = true;
                    break;
                }
            }
        }
        
    }
}