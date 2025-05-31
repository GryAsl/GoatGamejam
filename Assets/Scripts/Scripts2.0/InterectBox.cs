using Unity.Burst.CompilerServices;
using UnityEngine;

public class InterectBox : MonoBehaviour
{
    public GameObject item;
    public GameObject Kitchenware;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.GetComponent<Collider>().CompareTag("Item"))
        {
            item = other.gameObject;
        }
        if (other.gameObject.CompareTag("Interactable"))
        {
            Kitchenware = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Kitchenware = null;
    }
}
