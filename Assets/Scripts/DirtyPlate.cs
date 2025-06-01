using UnityEngine;

public class DirtyPlate : MonoBehaviour
{
    // This class can be expanded to add functionality specific to dirty plates
    // For example, you could add a cleanup mechanic where the player can pick up dirty plates
    
    void Start()
    {
        // Make sure the dirty plate has a collider
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("DirtyPlate needs a Collider component!");
            gameObject.AddComponent<BoxCollider>();
        }
        
        // Set the layer for the dirty plate
        gameObject.layer = LayerMask.NameToLayer("Dish");
    }
}
