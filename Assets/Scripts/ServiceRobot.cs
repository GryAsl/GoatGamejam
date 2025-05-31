using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ServiceRobot : MonoBehaviour
{
    [Header("NavMesh & Movement")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Plate Info")]
    [SerializeField] private Transform plateHoldPosition;
    
    [Header("Counters")]
    [SerializeField] private List<Transform> counters;
    [SerializeField] private float counterCheckRadius = 1.5f;
    [SerializeField] private LayerMask plateLayer;

    [Header("Tables")]
    [SerializeField] private List<TableController> tables;
    
    [Header("Idle Settings")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private float maxIdleTime = 10f;

    private bool isCarryingPlate;
    private Transform currentTarget;
    private GameObject currentPlateObject;
    private Foods currentFoodNeeded;
    private TableController currentTable;
    private float idleTimer = 0f;
    private Vector3 startPos;
    
    private enum RobotState { Idle, MovingToCounter, PickingUp, MovingToTable, DroppingOff, ReturningToStart }
    private RobotState currentState = RobotState.Idle;

    void Start()
    {
        Debug.Log("ServiceRobot Start - Initializing components");
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
            Debug.Log("NavMeshAgent component found: " + (agent != null));
        }
        
        if (!plateHoldPosition)
        {
            GameObject holdPos = new GameObject("PlateHoldPosition");
            holdPos.transform.parent = transform;
            holdPos.transform.localPosition = new Vector3(0, 1.2f, 0.3f);
            plateHoldPosition = holdPos.transform;
            Debug.Log("Created new plate hold position");
        }
        
        if (startPosition != null)
        {
            startPos = startPosition.position;
        }
        else
        {
            startPos = transform.position;
        }
        
        Debug.Log($"ServiceRobot initialized with {tables.Count} tables and {counters.Count} counters");
    }

    void Update()
    {
        switch (currentState)
        {
            case RobotState.Idle:
                idleTimer += Time.deltaTime;
                
                TableController tableNeedingPlate = FindTableNeedingPlate();
                if (tableNeedingPlate != null)
                {
                    Debug.Log($"Found table needing plate: {tableNeedingPlate.name}");
                    idleTimer = 0f;
                    currentTable = tableNeedingPlate;
                    currentFoodNeeded = tableNeedingPlate.GetRequestedFood();
                    Debug.Log($"Table needs food: {currentFoodNeeded.foodName}");
                    
                    Transform counterWithPlate = GetCounterWithFood(currentFoodNeeded);
                    if (counterWithPlate != null)
                    {
                        Debug.Log($"Found counter with needed food at: {counterWithPlate.name}");
                        currentTarget = counterWithPlate;
                        currentState = RobotState.MovingToCounter;
                        agent.SetDestination(currentTarget.position);
                    }
                    else
                    {
                        Debug.LogWarning("No counter found with the needed food");
                    }
                }
                else if (idleTimer >= maxIdleTime && Vector3.Distance(transform.position, startPos) > 0.5f)
                {
                    Debug.Log("Idle timeout, returning to start position");
                    currentState = RobotState.ReturningToStart;
                    agent.SetDestination(startPos);
                }
                break;
            
            case RobotState.MovingToCounter:
                if (ReachedDestination(currentTarget))
                {
                    Debug.Log("Reached counter, starting to pick up");
                    currentState = RobotState.PickingUp;
                }
                break;
            
            case RobotState.PickingUp:
                GameObject plate = FindPlateAtCounter(currentTarget, currentFoodNeeded);
                
                if (plate != null)
                {
                    Debug.Log($"Found plate with food: {currentFoodNeeded.foodName}");
                    currentPlateObject = plate;
                    currentPlateObject.transform.SetParent(plateHoldPosition);
                    currentPlateObject.transform.localPosition = Vector3.zero;
                    currentPlateObject.transform.localRotation = Quaternion.identity;
                    
                    isCarryingPlate = true;
                    
                    currentTarget = currentTable.transform;
                    agent.SetDestination(currentTarget.position);
                    currentState = RobotState.MovingToTable;
                }
                else
                {
                    Debug.LogWarning("Plate not found at counter, returning to start position");
                    currentState = RobotState.ReturningToStart;
                    agent.SetDestination(startPos);
                }
                break;

            case RobotState.MovingToTable:
                if (ReachedDestination(currentTarget))
                {
                    Debug.Log("Reached table, starting to drop off");
                    currentState = RobotState.DroppingOff;
                }
                break;
            
            case RobotState.DroppingOff:
                if (isCarryingPlate && currentPlateObject != null)
                {
                    Debug.Log("Dropping off plate at table");
                    Transform platePoint = currentTable.GetPlatePoint();
                    if (platePoint != null)
                    {
                        currentPlateObject.transform.SetParent(platePoint);
                        currentPlateObject.transform.localPosition = Vector3.zero;
                        currentPlateObject.transform.localRotation = Quaternion.identity;
                        
                        currentTable.PlateDelivered(currentFoodNeeded);
                    }
                    else
                    {
                        Debug.LogWarning("No plate point found on table, using table transform");
                        currentPlateObject.transform.SetParent(currentTable.transform);
                        currentPlateObject.transform.localPosition = new Vector3(0, 1f, 0); 
                        
                        currentTable.PlateDelivered(currentFoodNeeded);
                    }
                    
                    isCarryingPlate = false;
                    currentPlateObject = null;
                    currentTable = null;
                }
                
                currentState = RobotState.Idle;
                idleTimer = 0f; 
                break;
                
            case RobotState.ReturningToStart:
                if (ReachedDestination(startPos))
                {
                    Debug.Log("Returned to start position");
                    transform.position = startPos;
                    transform.rotation = Quaternion.identity;
                    
                    currentState = RobotState.Idle;
                    idleTimer = 0f; 
                }
                break;
        }
    }
    
    private TableController FindTableNeedingPlate()
    {
        foreach (TableController table in tables)
        {
            if (table.NeedsPlate())
            {
                Debug.Log($"Found table needing plate: {table.name}");
                return table;
            }
        }
        return null;
    }

    private GameObject FindPlateAtCounter(Transform counter, Foods food)
    {
        Debug.Log($"Searching for plate with food {food.foodName} at counter {counter.name}");
        Collider[] colliders = Physics.OverlapSphere(counter.position, counterCheckRadius);
        
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Plate"))
            {
                Plate plate = col.GetComponent<Plate>();
                if (plate != null && plate.foodsOnPlate.Exists(f => f.foodData == food))
                {
                    Debug.Log($"Found matching plate at counter {counter.name}");
                    return col.gameObject;
                }
            }
        }
        
        Debug.LogWarning($"No matching plate found at counter {counter.name}");
        return null;
    }

    private Transform GetCounterWithFood(Foods food)
    {
        foreach (Transform counter in counters)
        {
            GameObject plate = FindPlateAtCounter(counter, food);
            if (plate != null)
            {
                return counter;
            }
        }
        
        return null;
    }

    private bool ReachedDestination(Transform target)
    {
        if (!target) return false;
        
        return ReachedDestination(target.position);
    }
    
    private bool ReachedDestination(Vector3 targetPosition)
    {
        return !agent.pathPending && 
               agent.remainingDistance <= agent.stoppingDistance && 
               (!agent.hasPath || agent.velocity.sqrMagnitude < 0.1f);
    }
    
    private void OnDrawGizmos()
    {
        if (counters != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform counter in counters)
            {
                if (counter != null)
                {
                    Gizmos.DrawWireSphere(counter.position, counterCheckRadius);
                }
            }
        }
        
        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.position);
        }
        
        if (startPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPosition.position, 0.3f);
        }
    }
}