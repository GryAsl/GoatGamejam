using UnityEngine;
using System.Collections.Generic;

public class Plate : MonoBehaviour
{
    public List<Food> foodsOnPlate = new List<Food>();
    public Vector3[] foodPositions = new Vector3[] 
    {
        new Vector3(-0.1f, 0.1f, 0),  // Sol pozisyon
        new Vector3(0.1f, 0.1f, 0),   // Sağ pozisyon
    };

    public bool CanAddFood(Food food)
    {
        bool canAdd = foodsOnPlate.Count < 2;
        Debug.Log($"CanAddFood called: foodsOnPlate.Count={foodsOnPlate.Count}, canAdd={canAdd}");
        return canAdd;
    }

    public void AddFood(Food food)
    {

        Debug.Log($"AddFood called: food={food?.name}, foodsOnPlate.Count={foodsOnPlate.Count}");
        //if (!CanAddFood(food)) {
        //    Debug.Log("AddFood: Cannot add, plate is full.");
        //    return;
        //}

        foodsOnPlate.Add(food);
        food.transform.SetParent(transform);
        food.transform.localPosition = foodPositions[foodsOnPlate.Count - 1];
        food.transform.localRotation = Quaternion.identity;
        food.isOnPlate = true;
        Collider col = food.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Debug.Log($"AddFood: Food {food.name} added to plate at position {food.transform.localPosition}");
    }

    public void RemoveFood(Food food)
    {
        if (foodsOnPlate.Contains(food))
        {
            foodsOnPlate.Remove(food);
            food.transform.SetParent(null);
        }
    }

    void Start()
    {
        // Make sure the plate has a collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("Plate needs a Collider component!");
        }
        
        // Make sure the plate has the correct tag
        if (gameObject.tag != "Plate")
        {
            Debug.LogWarning("Plate should have 'Plate' tag!");
        }
    }
} 