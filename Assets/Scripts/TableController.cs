using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Transform platePoint; // Tabağın konulacağı nokta
    [SerializeField] private Foods requestedFood; // İstenen yemek
    [SerializeField] private bool needsPlate = false; // Tabak isteği var mı?
    
    void Start()
    {
        Debug.Log($"Table {name} initialized. Has plate point: {platePoint != null}");
    }
    
    // İstenen yemek
    public Foods GetRequestedFood()
    {
        Debug.Log($"Table {name} GetRequestedFood called. Food: {(requestedFood != null ? requestedFood.foodName : "null")}");
        return requestedFood;
    }
    
    // Tabağın konulacağı nokta
    public Transform GetPlatePoint()
    {
        return platePoint;
    }
    
    // Tabak isteği var mı?
    public bool NeedsPlate()
    {
        //Debug.Log($"Table {name} NeedsPlate called. Needs plate: {needsPlate}, Requested food: {(requestedFood != null ? requestedFood.foodName : "null")}");
        return needsPlate;
    }
    
    // Reference to the customer associated with this table
    private Customer associatedCustomer;
    public GameObject dirtyPlatePrefab;
    
    // Set the customer associated with this table
    public void SetCustomer(Customer customer)
    {
        associatedCustomer = customer;
    }
    
    // Tabak geldiğinde çağrılacak
    public void PlateDelivered(Foods food)
    {
        Debug.Log($"Table {name} PlateDelivered called with food: {food.foodName}");
        if (food == requestedFood)
        {
            needsPlate = false;
            Debug.Log($"Table {name} received requested food: {food.foodName}");
            
            // Notify the customer that the plate has been delivered
            if (associatedCustomer != null)
            {
                associatedCustomer.OnPlateDelivered(GetComponentInChildren<Plate>());
            }
        }
        else
        {
            Debug.LogWarning($"Table {name} received wrong food. Expected: {(requestedFood != null ? requestedFood.foodName : "null")}, Got: {food.foodName}");
        }
    }
    
    // Tabak isteği oluşturmak için
    public void RequestFood(Foods food)
    {
        Debug.Log($"Table {name} RequestFood called with food: {food.foodName}");
        requestedFood = food;
        needsPlate = true;
        Debug.Log($"Table {name} is requesting food: {food.foodName}, needsPlate set to: {needsPlate}");
    }
}