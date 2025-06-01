using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MyKitchenware : MonoBehaviour
{
    public string KitchenwareName;
    public bool cooking;
    public bool alreadyCooked;

    public GameObject food1;
    public GameObject food2;
    public GameObject food3;
    public GameObject food4;
    public GameObject currentFood;
    public GameObject cookedMeat;
    public GameObject burnedMeat;
    public Transform meatPoint;

    bool cookedMeatSpawned;
    public bool burnedMeatSpawned;
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
        Debug.LogWarning("1");
        currentFood = food;
        cooking = true;
        float time = 0f;
        if (food == food1 || food == food2)
        {
            while (time < 1f)
            {
                Debug.LogWarning("2");
                time += Time.deltaTime;
                yield return null;
            }
            cooking = false;
            alreadyCooked = true;
        }
        else if (food == food3)
        {
            cookedMeat = Instantiate(food3);
            cookedMeat.transform.position = meatPoint.position;
            cookedMeat.transform.SetParent(meatPoint);
            cookedMeat.SetActive(false);
            GameObject cookingMeat = Instantiate(food1);
            cookingMeat.transform.position = meatPoint.position;
            cookingMeat.transform.SetParent(meatPoint);
            while (time < 30f)
            {
                if (!cooking && !alreadyCooked)
                {
                    DestroyImmediate(cookedMeat);
                    DestroyImmediate(burnedMeat);
                    break;
                }
                Debug.LogWarning("3 :" + time);
                time += Time.deltaTime;
                if (time > 3f && time < 3.5f)
                {
                    if (!cookedMeatSpawned)
                    {
                        Debug.LogError("AHHH");
                        DestroyImmediate(cookingMeat);
                        cookedMeat.SetActive(true);

                        cooking = false;
                        alreadyCooked = true;
                        cookedMeatSpawned = true;
                    }

                }
                else if (time > 6f && time < 6.5f)
                {
                    if (!burnedMeatSpawned)
                    {
                        Debug.LogError("ZORt");
                        DestroyImmediate(cookedMeat);
                        burnedMeat = Instantiate(food4);
                        burnedMeat.transform.position = meatPoint.position;
                        burnedMeat.transform.SetParent(meatPoint);
                        burnedMeatSpawned = true;
                        alreadyCooked = false;
                    }

                }
                yield return null;
            }
        }


    }

    public GameObject SpawnFood(Transform GO)
    {
        GameObject newFood = Instantiate(currentFood);
        newFood.transform.position = GO.position;
        newFood.transform.SetParent(GO);

        return newFood;
    }

    public GameObject SpawnFood(Transform GO, GameObject foodToSpawn)
    {
        foodToSpawn = currentFood;
        GameObject newFood = Instantiate(currentFood);
        newFood.transform.position = GO.position;
        newFood.transform.SetParent(GO);

        return newFood;
    }
}


