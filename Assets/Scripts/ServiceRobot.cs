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
    private string currentPlateTypeNeeded;
    private TableController currentTable;
    private float idleTimer = 0f;
    private Vector3 startPos;
    
    private enum RobotState { Idle, MovingToCounter, PickingUp, MovingToTable, DroppingOff, ReturningToStart }
    private RobotState currentState = RobotState.Idle;

    void Start()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        if (!agent)
        {
        }
        
        if (!plateHoldPosition)
        {
            GameObject holdPos = new GameObject("PlateHoldPosition");
            holdPos.transform.parent = transform;
            holdPos.transform.localPosition = new Vector3(0, 1.2f, 0.3f);
            plateHoldPosition = holdPos.transform;
        }
        
        if (startPosition != null)
        {
            startPos = startPosition.position;
        }
        else
        {
            startPos = transform.position;
        }
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
                    idleTimer = 0f;
                    currentTable = tableNeedingPlate;
                    currentPlateTypeNeeded = tableNeedingPlate.GetRequestedPlateType();
                    
                    Transform counterWithPlate = GetCounterWithPlateType(currentPlateTypeNeeded);
                    if (counterWithPlate != null)
                    {
                        currentTarget = counterWithPlate;
                        currentState = RobotState.MovingToCounter;
                        agent.SetDestination(currentTarget.position);
                    }
                    else
                    {
                    }
                }
                else if (idleTimer >= maxIdleTime && Vector3.Distance(transform.position, startPos) > 0.5f)
                {
                    currentState = RobotState.ReturningToStart;
                    agent.SetDestination(startPos);
                }
                break;
            
            case RobotState.MovingToCounter:
                if (ReachedDestination(currentTarget))
                {
                    currentState = RobotState.PickingUp;
                }
                break;
            
            case RobotState.PickingUp:
                GameObject plate = FindPlateAtCounter(currentTarget, currentPlateTypeNeeded);
                
                if (plate != null)
                {
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
                    currentState = RobotState.Idle;
                    idleTimer = 0f; 
                }
                break;

            case RobotState.MovingToTable:
                if (ReachedDestination(currentTarget))
                {
                    currentState = RobotState.DroppingOff;
                }
                break;
            
            case RobotState.DroppingOff:
                if (isCarryingPlate && currentPlateObject != null)
                {
                    Transform platePoint = currentTable.GetPlatePoint();
                    if (platePoint != null)
                    {
                        currentPlateObject.transform.SetParent(platePoint);
                        currentPlateObject.transform.localPosition = Vector3.zero;
                        currentPlateObject.transform.localRotation = Quaternion.identity;
                        
                        currentTable.PlateDelivered(currentPlateTypeNeeded);
                        
                    }
                    else
                    {
                        currentPlateObject.transform.SetParent(currentTable.transform);
                        currentPlateObject.transform.localPosition = new Vector3(0, 1f, 0); 
                        
                        currentTable.PlateDelivered(currentPlateTypeNeeded);
                        
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
                    transform.position = startPos;
                    
                    if (startPosition != null)
                    {
                        transform.rotation = startPosition.rotation;
                    }
                    
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
                return table;
            }
        }
        return null;
    }

    private GameObject FindPlateAtCounter(Transform counter, string plateType)
    {
        Collider[] colliders = Physics.OverlapSphere(counter.position, counterCheckRadius, plateLayer);
        
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Plate"))
            {
                if (col.gameObject.name.Contains(plateType))
                {
                    return col.gameObject;
                }
            }
        }
        
        return null;
    }

    private Transform GetCounterWithPlateType(string plateType)
    {
        foreach (Transform counter in counters)
        {
            GameObject plate = FindPlateAtCounter(counter, plateType);
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