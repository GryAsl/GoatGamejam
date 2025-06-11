using Unity.Burst.CompilerServices;
using UnityEngine;

public class InterectBox : MonoBehaviour
{
    public GameObject arrow;
    public GameObject item;
    public GameObject Kitchenware;
    public GameObject Plate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Kitchenware != null)
        {
            arrow.GetComponent<MeshRenderer>().enabled = true;
            arrow.transform.position = new Vector3(Kitchenware.transform.position.x, Kitchenware.transform.position.y + 1.5f, Kitchenware.transform.position.z);
        }
        else if (item != null)
        {
            arrow.GetComponent<MeshRenderer>().enabled = true;
            arrow.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + 1.5f, item.transform.position.z);
        }
        else if (Plate != null)
        {
            arrow.GetComponent<MeshRenderer>().enabled = true;
            arrow.transform.position = new Vector3(Plate.transform.position.x, Plate.transform.position.y + 1.5f, Plate.transform.position.z);
        }
        else arrow.GetComponent<MeshRenderer>().enabled = false;
    }
}
