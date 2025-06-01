using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public Foods[] foodList;
    public Foods[] lastTwoFoods;
    public GameObject[] customerPrefabs;
    public PointData[] pointData;
    public bool isAllTablesAreFull;
    public float gameEndTimer;

    // Add a reference to the coroutine
    private Coroutine spawnCoroutine;

    void Start()
    {

    }

    public void StartSpawning()
    {
        // Start spawning customers
        spawnCoroutine = StartCoroutine(SpawnCustomerRoutine());
    }

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

    IEnumerator SpawnCustomerRoutine()
    {
        while (true)
        {
            // 15 saniye bekle
            yield return new WaitForSeconds(Random.Range(5f,10f));
            Debug.LogWarning("1");
            
            foreach (PointData pointDataCurrent in pointData)
            {
                Debug.LogWarning(pointDataCurrent.isEmpty);
                if (pointDataCurrent.isEmpty)
                {
                    isAllTablesAreFull = false;
                    break;
                }
                else isAllTablesAreFull = true;
            }

            if (!isAllTablesAreFull)
            {
                NewCustomer();
            }
        }
    }

    public void NewCustomer()
    {
        PointData currentPointData = pointData[Random.Range(0, pointData.Length)];

        

        int Customers = Random.Range(0, customerPrefabs.Length);
        Customer customer = Instantiate(customerPrefabs[Customers]).GetComponent<Customer>();
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
        
        
        //buraya level1den sonra tekrar açılma getirmemiz lazım.
        int i;
        if (GameObject.Find("GameManager").GetComponent<GameManager>().isLevelUP)
        {
            i = Random.Range(0, foodList.Length);
        }
        else
        {
            do
            {
                i = Random.Range(0, foodList.Length);
            } while (i == 0);
        }


        if (currentPointData.isEmpty)
        {
            Debug.LogWarning("1");
            customer.NewOrder(foodList[i], currentPointData.point, currentPointData.table);
            currentPointData.isEmpty = false;
            currentPointData.table.RequestFood(foodList[i]);
        }
        else
        {
            foreach (PointData pointDataCurrent in pointData)
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


    }
}

[System.Serializable]
public class PointData
{
    public GameObject point;
    public TableController table;
    public bool isEmpty;
}