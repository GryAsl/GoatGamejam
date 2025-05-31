using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2f;
    public LayerMask interactableLayer;

    private CharacterController controller;
    private Vector3 movement;
    private Animator animator;
    private bool isInteracting;

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!gm.isGameOn) return;

        if (isInteracting) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Update animation parameters
        animator.SetFloat("Speed", movement.magnitude);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp( transform.rotation, toRotation, Time.deltaTime * 12f);
        }

        controller.Move(transform.forward * movement.magnitude * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Interact()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            isInteracting = true;
           
            Debug.Log(hit.collider);
        }
    }

    // Animation event callback
    public void OnInteractComplete()
    {
        isInteracting = false;
    }
}
