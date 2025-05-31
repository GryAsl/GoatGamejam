using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public Foods[] foodList;
    public Foods[] lastTwoFoods;
    public GameObject[] customerPrefabs;
    public PointData[] pointData;
    public bool isAllTablesAreFull;
    public float gameEndTimer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAllTablesAreFull)
        {
            gameEndTimer += Time.deltaTime;
        }
        else
        {
            gameEndTimer = 0f;
        }
    }

    public void NewCustomer()
    {
        PointData currentPointData = pointData[Random.Range(0, pointData.Length)];
        int i = Random.Range(0, foodList.Length);


        Customer customer = Instantiate(customerPrefabs[0]).GetComponent<Customer>();
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();

        if (currentPointData.isEmpty)
        {
                    Debug.LogWarning("1");
            customer.NewOrder(foodList[i], currentPointData.point, currentPointData.table);
            currentPointData.isEmpty = false;
            currentPointData.table.RequestFood(foodList[i]);
        }
        else
        {
            foreach( PointData pointDataCurrent in pointData)
            {
                    Debug.LogWarning(pointDataCurrent.isEmpty);
                if (pointDataCurrent.isEmpty)
                {
                    if (pointDataCurrent.table != null)
                    {
                        pointDataCurrent.table.RequestFood(foodList[i]);
                    }
                    customer.NewOrder(foodList[i], pointDataCurrent.point, pointDataCurrent.table);
                    pointDataCurrent.isEmpty = false;
                    break;
                }
            }
        }

        foreach (PointData pointDataCurrent in pointData)
        {
            Debug.LogWarning(pointDataCurrent.isEmpty);
            if (pointDataCurrent.isEmpty)
            {
                isAllTablesAreFull = false;
            }
            else isAllTablesAreFull = true;
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
