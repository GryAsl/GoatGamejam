using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public Foods[] foodList;
    public GameObject[] customerPrefabs;
    public PointData[] pointData;

    void Start()
    {
        Debug.Log($"CustomerManager initialized with {foodList.Length} foods and {pointData.Length} points");
        foreach (var point in pointData)
        {
            Debug.Log($"Point: {point.point.name}, Has table: {point.table != null}, Is empty: {point.isEmpty}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewCustomer()
    {
        PointData currentPointData = pointData[Random.Range(0, pointData.Length)];
        int i = Random.Range(0, foodList.Length);

        Debug.Log($"Creating new customer with food: {foodList[i].foodName} at point: {currentPointData.point.name}");
        Debug.Log($"Point has table: {currentPointData.table != null}");

        if (currentPointData.table != null)
        {
            currentPointData.table.RequestFood(foodList[i]);
            Debug.Log($"Table {currentPointData.table.name} requested food: {foodList[i].foodName}");
        }

        Customer cus = Instantiate(customerPrefabs[0]).GetComponent<Customer>();
        NavMeshAgent agent = cus.GetComponent<NavMeshAgent>();
        Debug.Log($"Customer created with stopping distance: {agent.stoppingDistance}");

        if (currentPointData.isEmpty)
        {
            Debug.Log($"Assigning customer to empty point: {currentPointData.point.name}");
            cus.NewOrder(foodList[i], currentPointData.point, currentPointData.table);
            currentPointData.isEmpty = false;
        }
        else
        {
            foreach( PointData pointDataCurrent in pointData)
            {
                if (pointDataCurrent.isEmpty)
                {
                    Debug.Log($"Assigning customer to empty point: {pointDataCurrent.point.name}");
                    if (pointDataCurrent.table != null)
                    {
                        pointDataCurrent.table.RequestFood(foodList[i]);
                        Debug.Log($"Table {pointDataCurrent.table.name} requested food: {foodList[i].foodName}");
                    }
                    cus.NewOrder(foodList[i], pointDataCurrent.point, pointDataCurrent.table);
                    pointDataCurrent.isEmpty = false;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class PointData
{
    public GameObject point;
    public TableController table;
    public bool isEmpty;
}
