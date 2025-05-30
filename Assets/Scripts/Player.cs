using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2f;
    public LayerMask interactableLayer;

    private CharacterController controller;
    private Vector3 movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1440f * Time.deltaTime);
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
            Debug.Log(hit.collider);
        }
    }
}
