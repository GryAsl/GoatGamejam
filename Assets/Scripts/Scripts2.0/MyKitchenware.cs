using System.Collections;
using UnityEngine;

public class MyKitchenware : MonoBehaviour
{
    public string KitchenwareName;
    public bool cooking;
    public bool alreadyCooked;

    public GameObject food1;
    public GameObject food2;
    public GameObject food3;
    public GameObject currentFood;
    public Plate plate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartCooking(GameObject food)
    {
        currentFood = food;
        cooking = true;
        float time = 0f;
        while(time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        cooking = false;
        alreadyCooked = true;
    }

    public GameObject SpawnFood(Transform GO)
    {
        GameObject newFood = Instantiate(currentFood);
        newFood.transform.position = GO.position;
        newFood.transform.SetParent(GO);

        return newFood;
    }
}


