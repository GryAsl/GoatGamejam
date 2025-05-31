using Unity.Burst.CompilerServices;
using UnityEngine;

public class InterectBoxChild : MonoBehaviour
{
    public InterectBox intBox;
    public int type;

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
        switch (type)
        {
            case  0:
                Debug.Log(other);
                intBox.item = other.gameObject;
                break;
            case 1:

                if (other.CompareTag("Interactable"))
                {
                    Debug.Log(other);
                    intBox.Kitchenware = other.gameObject;
                    break;
                }
                break;

            case 2:
                if (other.CompareTag("Plate"))
                {
                    Debug.Log(other);
                    intBox.Plate = other.gameObject;
                    break;
                }
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        switch (type)
        {
            case 0:
                intBox.item = null;
                break;
            case 1:
                if (other.CompareTag("Interactable"))
                {
                    Debug.Log(other);
                    intBox.Kitchenware = null;
                    break;
                }
                break;
            case 2:
                if (other.CompareTag("Plate"))
                {
                    Debug.Log(other);
                    intBox.Plate = null;
                    break;
                }
                break;
        }
    }
}
