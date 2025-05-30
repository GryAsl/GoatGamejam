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

    private bool isCarryingPlate;
    private Transform currentTarget;
    private GameObject currentPlateObject;
    private string currentPlateTypeNeeded;
    private TableController currentTable;
    
    private enum RobotState { Idle, MovingToCounter, PickingUp, MovingToTable, DroppingOff }
    private RobotState currentState = RobotState.Idle;

    void Start()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        
        if (!agent)
        {
            Debug.LogWarning("No NavMeshAgent found on ServiceRobot. Please assign one.");
        }
        
        if (!plateHoldPosition)
        {
            // Create a default plate hold position if none exists
            GameObject holdPos = new GameObject("PlateHoldPosition");
            holdPos.transform.parent = transform;
            holdPos.transform.localPosition = new Vector3(0, 1.2f, 0.3f);
            plateHoldPosition = holdPos.transform;
            Debug.LogWarning("No plate hold position assigned. Created a default one.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case RobotState.Idle:
                // Tabak isteyen masa var mı kontrol et
                TableController tableNeedingPlate = FindTableNeedingPlate();
                if (tableNeedingPlate != null)
                {
                    currentTable = tableNeedingPlate;
                    currentPlateTypeNeeded = tableNeedingPlate.GetRequestedPlateType();
                    
                    // Bu tip tabağın olduğu bir counter bul
                    Transform counterWithPlate = GetCounterWithPlateType(currentPlateTypeNeeded);
                    if (counterWithPlate != null)
                    {
                        currentTarget = counterWithPlate;
                        currentState = RobotState.MovingToCounter;
                        agent.SetDestination(currentTarget.position);
                        Debug.Log("Moving to counter to pick up " + currentPlateTypeNeeded + " for table " + currentTable.name);
                    }
                    else
                    {
                        Debug.Log("No counter has the needed plate type: " + currentPlateTypeNeeded);
                    }
                }
                break;
            
            case RobotState.MovingToCounter:
                if (ReachedDestination(currentTarget))
                {
                    currentState = RobotState.PickingUp;
                    Debug.Log("Reached counter, attempting to pick up plate");
                }
                break;
            
            case RobotState.PickingUp:
                // Counter alanındaki tabakları kontrol et
                GameObject plate = FindPlateAtCounter(currentTarget, currentPlateTypeNeeded);
                
                if (plate != null)
                {
                    // Tabağı al
                    currentPlateObject = plate;
                    currentPlateObject.transform.SetParent(plateHoldPosition);
                    currentPlateObject.transform.localPosition = Vector3.zero;
                    currentPlateObject.transform.localRotation = Quaternion.identity;
                    
                    isCarryingPlate = true;
                    Debug.Log("Picked up plate: " + currentPlateTypeNeeded);
                    
                    // Tabağı götüreceğimiz masanın plate point'i
                    Transform tablePoint = currentTable.GetPlatePoint();
                    if (tablePoint != null)
                    {
                        currentTarget = tablePoint;
                        agent.SetDestination(currentTarget.position);
                        currentState = RobotState.MovingToTable;
                        Debug.Log("Moving to table " + currentTable.name + " to deliver plate");
                    }
                    else
                    {
                        Debug.LogWarning("No plate point found on table " + currentTable.name);
                        currentState = RobotState.Idle;
                    }
                }
                else
                {
                    Debug.LogWarning("No plate of type " + currentPlateTypeNeeded + " found at counter!");
                    currentState = RobotState.Idle;
                }
                break;

            case RobotState.MovingToTable:
                if (ReachedDestination(currentTarget))
                {
                    currentState = RobotState.DroppingOff;
                    Debug.Log("Reached table, dropping off plate");
                }
                break;
            
            case RobotState.DroppingOff:
                if (isCarryingPlate && currentPlateObject != null)
                {
                    // Tabağı masaya yerleştir
                    currentPlateObject.transform.SetParent(currentTarget);
                    currentPlateObject.transform.localPosition = Vector3.zero;
                    currentPlateObject.transform.localRotation = Quaternion.identity;
                    
                    // Masaya tabak geldiğini bildir
                    currentTable.PlateDelivered(currentPlateTypeNeeded);
                    
                    Debug.Log("Plate delivered to table " + currentTable.name);
                    isCarryingPlate = false;
                    currentPlateObject = null;
                    currentTable = null;
                }
                
                // Idle state'e dön
                currentState = RobotState.Idle;
                break;
        }
    }

    // Tabak isteyen bir masa bul
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
        // Counter çevresindeki tüm collider'ları bul
        Collider[] colliders = Physics.OverlapSphere(counter.position, counterCheckRadius, plateLayer);
        
        foreach (Collider col in colliders)
        {
            // Tag'i "Plate" olan objeleri kontrol et
            if (col.CompareTag("Plate"))
            {
                // İsim üzerinden plate tipini kontrol et
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
        
        // NavMeshAgent'a göre hedefe ulaşıp ulaşmadığımızı kontrol et
        return !agent.pathPending && 
               agent.remainingDistance <= agent.stoppingDistance && 
               (!agent.hasPath || agent.velocity.sqrMagnitude < 0.1f);
    }

    // Counter check alanını ve mevcut hedefi görselleştir
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
    }
}