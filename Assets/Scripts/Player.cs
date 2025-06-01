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
        if (currentItem == null && box.item != null)
        {
            currentItem = box.item;
            currentItem.transform.position = itemTransform.position;
            currentItem.gameObject.transform.SetParent(gameObject.transform);
            currentItem.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "Plate" && currentItem.GetComponent<MyItem>().itemName == "Meat")
        {
            currentKitchenware.GetComponent<MyKitchenware>().plate.AddFood(currentItem.GetComponent<Food>());
        }
        else if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "3D" && currentKitchenware.GetComponent<MyKitchenware>().alreadyCooked)
        {
                currentKitchenware.GetComponent<MyKitchenware>().SpawnFood(itemTransform);
        }
        else
        {
            Debug.Log(currentKitchenware);
            Debug.Log(currentItem);
            if (currentKitchenware.GetComponent<MyKitchenware>().KitchenwareName == "3D" && currentItem.GetComponent<MyItem>().itemName == "Filement")
            {
                DestroyImmediate(currentItem);
                currentItem = null;
                box.item = null;
                StartCoroutine(currentKitchenware.GetComponent<MyKitchenware>().StartCooking());
            }

        }


    }

    // Animation event callback
    public void OnInteractComplete()
    {
        isInteracting = false;
    }
}
