using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float ascendSpeed = 3f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float hoverHeight = 5f;
    [SerializeField] private float landingThreshold = 0.1f;
    [SerializeField] private float pickupDistance = 1.5f;
    [SerializeField] public float AngularSpeed = 800f;
    
    [Header("Positions")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform dishesParent;
    
    [Header("Dish Detection")]
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private LayerMask dishLayer;
    
    [Header("References")]
    [SerializeField] private Transform startPositionRef;
    [SerializeField] private Transform dishesParentRef;
    [SerializeField] private WashMachine washMachine;
    [SerializeField] private Transform Pervane1;
    [SerializeField] private Transform Pervane2;
    [SerializeField] private Transform Pervane3;
    [SerializeField] private Transform Pervane4;

    
    private enum DroneState
    {
        Idle,
        TakingOff,
        MovingToDish,
        LandingAtDish,
        PickingUpDish,
        TakingOffWithDish,
        ReturningToStart,
        LandingAtStart,
        ReturnToStartIdle,
        ReturnToStartPosition
    }
    
    private DroneState currentState = DroneState.Idle;
    private Vector3 targetPosition;
    private Transform currentDishTarget;
    private List<GameObject> collectedDishes = new List<GameObject>();
    private List<Transform> detectedDishes = new List<Transform>();
    
    private Vector3 startPos;
    private Quaternion initialRotation;
    private float scanInterval = 3.0f; // Scan every 3 seconds
    private float lastScanTime = 0f;
    
    void Start()
    {
        if (startPosition == null)
        {
            startPos = transform.position;
        }
        else
        {
            startPos = startPosition.position;
        }
        
        initialRotation = transform.rotation;
        
        transform.position = startPos;
        
        if (dishesParent == null)
        {
            ScanForDishes();
        }
        else
        {
            foreach (Transform dish in dishesParent)
            {
                detectedDishes.Add(dish);
            }
        }
        
        currentState = DroneState.Idle;
        targetPosition = startPos;
    }
    
    private void ScanForDishes()
    {
        // Sahnede hala varolan eşyaları algıla
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, dishLayer);
        foreach (Collider collider in hitColliders)
        {
            // Çöpleri listeye ekle, zaten varsa ekleme, koleksiyonla ilgili kontrol yap
            if (!detectedDishes.Contains(collider.transform) && 
                !collectedDishes.Contains(collider.gameObject))
            {
                detectedDishes.Add(collider.transform);
            }
        }

        // Null kontrol veya sahneden kaldırılmış objeleri temizle
        detectedDishes.RemoveAll(dish => dish == null || !dish.gameObject.activeInHierarchy);
    }

    void Update()
    {
        if (Pervane1 != null)
            ((Transform)Pervane1).Rotate(0f,  Time.deltaTime * AngularSpeed, 0f );
        if (Pervane2 != null) 
            ((Transform)Pervane2).Rotate(0f,  Time.deltaTime * AngularSpeed, 0f );
        if (Pervane3 != null)
            ((Transform)Pervane3).Rotate(0f,  Time.deltaTime * AngularSpeed, 0f );
        if (Pervane4 != null)
            ((Transform)Pervane4).Rotate(0f,  Time.deltaTime * AngularSpeed, 0f);

        // Diğer mantıklarınızı koruyarak, listeleri sık sık temizlemek için null kontrolleri:
        detectedDishes.RemoveAll(dish => dish == null || !dish.gameObject.activeInHierarchy);
        collectedDishes.RemoveAll(dish => dish == null || !dish.activeInHierarchy);

        MaintainUpright();

        // Scan polled periodically regardless of the state
        if (Time.time - lastScanTime > scanInterval)
        {
            ScanForDishes();
            lastScanTime = Time.time;

            if (currentState == DroneState.Idle && detectedDishes.Count > 0)
            {
                currentState = DroneState.TakingOff;
                targetPosition = new Vector3(transform.position.x, hoverHeight, transform.position.z);
            }
        }

        // Diğer mevcut durum denetimleri burada:
        switch (currentState)
        {
            case DroneState.Idle:
            // Eski mantığınız burada devam ediyor...
            break;
                
            case DroneState.ReturnToStartPosition:
                if (MoveToPosition(startPos))
                {
                    transform.position = startPos;
                    currentState = DroneState.Idle;
                }
                break;
                
            case DroneState.ReturnToStartIdle:
                if (MoveToPosition(targetPosition))
                {
                    transform.position = startPos;
                    currentState = DroneState.Idle;
                }
                break;
                
            case DroneState.TakingOff:
                if (MoveToPosition(targetPosition))
                {
                    SelectNextDish();
                    currentState = DroneState.MovingToDish;
                }
                break;
                
            case DroneState.MovingToDish:
                if (currentDishTarget == null)
                {
                    if (detectedDishes.Count > 0)
                    {
                        SelectNextDish();
                    }
                    else
                    {
                        targetPosition = new Vector3(startPos.x, hoverHeight, startPos.z);
                        currentState = DroneState.ReturningToStart;
                    }
                    break;
                }
                
                targetPosition = new Vector3(
                    currentDishTarget.position.x,
                    hoverHeight,
                    currentDishTarget.position.z
                );
                
                if (MoveToPosition(targetPosition))
                {
                    targetPosition = new Vector3(
                        currentDishTarget.position.x,
                        currentDishTarget.position.y + landingThreshold,
                        currentDishTarget.position.z
                    );
                    currentState = DroneState.LandingAtDish;
                }
                break;
                
            case DroneState.LandingAtDish:
                if (MoveToPosition(targetPosition))
                {
                    currentState = DroneState.PickingUpDish;
                }
                break;
                
            case DroneState.PickingUpDish:
                if (currentDishTarget != null && 
                    Vector3.Distance(transform.position, currentDishTarget.position) < pickupDistance)
                {
                    PickUpDish(currentDishTarget.gameObject);
                    
                    targetPosition = new Vector3(transform.position.x, hoverHeight, transform.position.z);
                    currentState = DroneState.TakingOffWithDish;
                }
                else
                {
                    SelectNextDish();
                    if (currentDishTarget != null)
                    {
                        currentState = DroneState.MovingToDish;
                    }
                    else
                    {
                        targetPosition = new Vector3(startPos.x, hoverHeight, startPos.z);
                        currentState = DroneState.ReturningToStart;
                    }
                }
                break;
                
            case DroneState.TakingOffWithDish:
                if (MoveToPosition(targetPosition))
                {
                    targetPosition = new Vector3(startPos.x, hoverHeight, startPos.z);
                    currentState = DroneState.ReturningToStart;
                }
                break;
                
            case DroneState.ReturningToStart:
                if (MoveToPosition(targetPosition))
                {
                    targetPosition = startPos;
                    currentState = DroneState.LandingAtStart;
                }
                break;
                
            case DroneState.LandingAtStart:
                if (MoveToPosition(targetPosition))
                {
                    // Properly land the drone by setting it exactly at the start position
                    transform.position = startPos;
                    transform.rotation = initialRotation;
                    
                    foreach (GameObject dish in collectedDishes)
                    {
                        Destroy(dish);
                    }
                    collectedDishes.Clear();
                    
                    // Notify wash machine to start washing process
                    if (washMachine != null)
                    {
                        washMachine.StartWashing();
                    }
                    
                    // Always transition to Idle state after landing
                    currentState = DroneState.Idle;
                    
                    // Scan for new dishes while in idle state
                    ScanForDishes();
                }
                break;
        }
    }
    
    private bool MoveToPosition(Vector3 target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target);
        
        float arrivalThreshold = target == startPos ? 0.05f : 0.1f;
        
        if (distanceToTarget < arrivalThreshold)
        {
            if (target == startPos)
            {
                transform.position = startPos;
            }
            return true;
        }
        
        Vector3 direction = (target - transform.position).normalized;
        
        if (new Vector2(direction.x, direction.z).magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Vector3 eulerAngles = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        float speed = Mathf.Abs(target.y - transform.position.y) > 0.1f ? ascendSpeed : moveSpeed;
        
        if (target == startPos && distanceToTarget < 1.0f)
        {
            speed *= 0.5f;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        return false;
    }
    
    private void PickUpDish(GameObject dish)
    {
        if (dish != null)
        {
            dish.transform.SetParent(transform);
            
            dish.transform.localPosition = new Vector3(0, -0.5f, 0);
            
            collectedDishes.Add(dish);
            detectedDishes.Remove(dish.transform);
            
            currentDishTarget = null;
        }
    }
    
    private void SelectNextDish()
    {
        if (detectedDishes.Count > 0)
        {
            Transform closestDish = null;
            float closestDistance = float.MaxValue;
            
            foreach (Transform dish in detectedDishes)
            {
                if (dish != null)
                {
                    float distance = Vector3.Distance(transform.position, dish.position);
                    if (distance < closestDistance)
                    {
                        closestDish = dish;
                        closestDistance = distance;
                    }
                }
            }
            
            currentDishTarget = closestDish;
        }
        else
        {
            currentDishTarget = null;
        }
    }
    
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        if (currentDishTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentDishTarget.position);
        }
    }
    
    private void MaintainUpright()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        
        Quaternion targetRotation = Quaternion.Euler(0, currentRotation.y, 0);
        
        transform.rotation = targetRotation;
    }
}