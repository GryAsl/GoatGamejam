using System.Collections;
using UnityEngine;

public class MyKitchenware : MonoBehaviour
{
    public string KitchenwareName;
    public bool cooking;
    public bool alreadyCooked;

    public GameObject food;
    public Plate plate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartCooking()
    {
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

    public void SpawnFood(Transform GO)
    {
        GameObject newFood = Instantiate(food);
        newFood.transform.position = GO.position;
        newFood.transform.SetParent(GO);
    }
}


