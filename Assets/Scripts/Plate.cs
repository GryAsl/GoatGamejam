using UnityEngine;

public class Plate : MonoBehaviour
{
    public Foods food;
    
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
        
        // Make sure food is assigned
        if (food == null)
        {
            Debug.LogError("Plate needs a food assigned!");
        }
    }
} 