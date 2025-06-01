using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2f;
    public LayerMask interactableLayer;

    private CharacterController controller;
    private Vector3 movement;
    public Animator animator;
    private bool isInteracting;

    GameManager gm;

    public Transform itemTransform;
    public InterectBox box;
    public GameObject currentItem;
    public GameObject currentKitchenware;
    public GameObject red;
    public GameObject yellow;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //if (!gm.isGameOn) return;

        if (isInteracting) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Update animation parameters
        animator.SetFloat("Speed", movement.magnitude);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        controller.Move(transform.forward * movement.magnitude * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Interact()
    {
        currentKitchenware = box.Kitchenware;
        if (currentItem != null &&  currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Trash")
        {
            DestroyImmediate(currentItem);
            currentItem = null;
        }
        else if (currentItem == null && box.item == null && currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Yellow")
        {
            currentItem = Instantiate(yellow);
            currentItem.transform.position = itemTransform.position;
            currentItem.gameObject.transform.SetParent(itemTransform.transform);
        }
        else if (currentItem == null && box.item == null && currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Red")
        {
            currentItem = Instantiate(red);
            currentItem.transform.position = itemTransform.position;
            currentItem.gameObject.transform.SetParent(itemTransform.transform);
        }
        else if (currentItem == null && box.item != null)
        {
            currentItem = box.item;
            currentItem.transform.position = itemTransform.position;
            currentItem.gameObject.transform.SetParent(itemTransform.transform);
        }
        else if (box.Plate != null && currentItem.GetComponent<MyItem>().itemName == "Meat")
        {
            box.Plate.GetComponent<Plate>().AddFood(currentItem.GetComponent<Food>());
        }
        else if (box.Plate != null && currentItem.GetComponent<MyItem>().itemName == "MeatCooked")
        {
            box.Plate.GetComponent<Plate>().AddFood(currentItem.GetComponent<Food>());
        }
        else if (box.Plate != null && currentItem.GetComponent<MyItem>().itemName == "Pat")
        {
            box.Plate.GetComponent<Plate>().AddFood(currentItem.GetComponent<Food>());
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "3D" && currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked)
        {
            Debug.Log("AHHHHH");
            currentItem = currentKitchenware.GetComponent<MyKitchenware>().SpawnFood(itemTransform);
            currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked = false;
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Oven" && currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked)
        {
            Debug.Log("2");
            currentItem = currentKitchenware.GetComponent<MyKitchenware>().SpawnFood(itemTransform);
            currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked = false;
            currentKitchenware.GetComponent<MyKitchenware>().cooking = false;
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Oven" && currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked == false && currentKitchenware.GetComponent<MyKitchenware>().burnedMeatSpawned == true)
        {
            Debug.Log("3");
            currentItem = currentKitchenware.GetComponent<MyKitchenware>().SpawnFood(itemTransform, currentKitchenware.GetComponent<MyKitchenware>().food4);
            currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked = false;
            currentKitchenware.GetComponent<MyKitchenware>().cooking = false;
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Oven" && currentItem.GetComponent<MyItem>().itemName == "Meat")
        {
            DestroyImmediate(currentItem);
            currentItem = null;
            box.item = null;
            StartCoroutine(currentKitchenware.GetComponent<MyKitchenware>().StartCooking(currentKitchenware.GetComponent<MyKitchenware>().food3));
            return;
        }
        // else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Oven" && currentItem.GetComponent<MyItem>().itemName == "Meat")
        // {
        //     DestroyImmediate(currentItem);
        //     currentItem = null;
        //     box.item = null;
        //     StartCoroutine(currentKitchenware.GetComponent<MyKitchenware>().StartCooking(currentKitchenware.GetComponent<MyKitchenware>().food3));
        //     return;
        // }
        else
        {
            Debug.Log(currentKitchenware);
            Debug.Log(currentItem);
            if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "3D" && currentItem.GetComponent<MyItem>().itemName == "FilementEt")
            {
                DestroyImmediate(currentItem);
                currentItem = null;
                box.item = null;
                StartCoroutine(currentKitchenware.GetComponent<MyKitchenware>().StartCooking(currentKitchenware.GetComponent<MyKitchenware>().food1));
                return;
            }
            if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "3D" && currentItem.GetComponent<MyItem>().itemName == "FilementPat")
            {
                DestroyImmediate(currentItem);
                currentItem = null;
                box.item = null;
                StartCoroutine(currentKitchenware.GetComponent<MyKitchenware>().StartCooking(currentKitchenware.GetComponent<MyKitchenware>().food2));
                return;
            }

        }


    }

    // Animation event callback
    public void OnInteractComplete()
    {
        isInteracting = false;
    }
}
