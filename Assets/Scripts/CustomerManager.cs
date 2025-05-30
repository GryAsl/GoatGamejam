using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public Foods[] foodList;
    public GameObject[] customerPrefabs;
    public PointData[] pointData;




    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewCustomer()
    {
        PointData currentPointData = pointData[Random.Range(0, pointData.Length)];
        int i = Random.Range(0, foodList.Length);

        Customer cus = Instantiate(customerPrefabs[0]).GetComponent<Customer>();

        if (currentPointData.isEmpty)
        {
            cus.NewOrder(foodList[i], currentPointData.point);
            currentPointData.isEmpty = false;
        }
        else
        {
            foreach( PointData pointDataCurrent in pointData)
            {
                if (pointDataCurrent.isEmpty)
                {
                    cus.NewOrder(foodList[i], pointDataCurrent.point);
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
    public bool isEmpty;
}
