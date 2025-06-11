using Unity.Burst.CompilerServices;
using UnityEngine;

public class InterectBoxChild : MonoBehaviour
{
    public InterectBox intBox;
    public int type;
    public float currentDistance = 0f;
    public float nextDistance = 0f;

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
                intBox.item = other.gameObject;
                break;
            case 1:
                if (other.CompareTag("Interactable"))
                {
                    if (intBox.Kitchenware != null)
                    {
                        currentDistance = Vector3.Distance(intBox.Kitchenware.transform.position, transform.position);
                        nextDistance = Vector3.Distance(other.gameObject.transform.position, transform.position);
                        if (currentDistance < nextDistance)
                            return;
                    }
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

    private void OnTriggerStay(Collider other)
    {
        switch (type)
        {
            case 0:
                //if(intBox.item != null)
                //{
                //    currentDistance = Vector3.Distance(intBox.item.transform.position, transform.position);
                //    nextDistance = Vector3.Distance(other.gameObject.transform.position, transform.position);
                //    if (currentDistance < nextDistance)
                //        break;
                //}
                intBox.item = other.gameObject;
                break;
            case 1:
                if (other.CompareTag("Interactable"))
                {
                    if (intBox.Kitchenware != null)
                    {
                        currentDistance = Vector3.Distance(intBox.Kitchenware.transform.position, transform.position);
                        nextDistance = Vector3.Distance(other.gameObject.transform.position, transform.position);
                        if (currentDistance < nextDistance)
                            break;
                    }
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
