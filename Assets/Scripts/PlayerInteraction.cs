using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 0.6f;
    public Vector3 boxSize = new Vector3(0.4f, 0.4f, 0.4f);
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;

    [Header("Hold Settings")]
    public Transform holdPoint;
    private GameObject heldItem;
    private bool isHoldingItem = false;

    void Update()
    {
        HandleInteractionInput();
    }

    void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey))
        {
            // Eğer eşya tutuyorsa bırak, yoksa alma dene
            if (isHoldingItem)
                Drop();
            else
                TryInteract();
        }
    }

    void TryInteract()
    {
        Vector3 origin = transform.position + transform.forward * interactRange;

        Collider[] hits = Physics.OverlapBox(origin, boxSize * 0.5f, Quaternion.identity, interactableLayer);

        if (hits.Length == 0)
            return;

        foreach (Collider col in hits)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
                break;
            }
        }
    }

    public void PickUp(GameObject item)
    {
        if (isHoldingItem)
        {
            Debug.LogWarning("Already holding an item!");
            return;
        }

        heldItem = item;
        isHoldingItem = true;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    public void Drop()
    {
        if (!isHoldingItem) return;

        GameObject itemToDrop = heldItem;
        heldItem = null;
        isHoldingItem = false;

        // DropZone tespiti
        Vector3 origin = transform.position + transform.forward * interactRange;
        Collider[] hits = Physics.OverlapBox(origin, boxSize * 0.5f, Quaternion.identity, interactableLayer);

        Transform dropParent = null;
        foreach (Collider col in hits)
        {
            if (col.CompareTag("DropZone"))  // DropZone objelerine bu tag'ı ver!
            {
                dropParent = col.transform;
                break;
            }
        }

        // Eğer bir DropZone varsa, oraya parent et
        if (dropParent != null)
        {
            itemToDrop.transform.SetParent(dropParent);
            itemToDrop.transform.localPosition = Vector3.zero; // DropZone'un ortasına yerleştir
            itemToDrop.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Sahneye serbest bırak
            itemToDrop.transform.SetParent(null);
            itemToDrop.transform.position = holdPoint.position + transform.forward * 0.5f;
        }

        if (itemToDrop.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
    }

    public bool HasItem() => isHoldingItem;
    public GameObject GetHeldItem() => heldItem;

    // 🔍 Etkileşim alanını sahnede göster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + transform.forward * interactRange;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
