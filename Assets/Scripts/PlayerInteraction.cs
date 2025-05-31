using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 0.6f;
    public Vector3 boxSize = new Vector3(0.4f, 0.4f, 0.4f);
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;
    public float interactionHeight = 1f; // Etkileşim kutusunun yüksekliği

    [Header("Hold Settings")]
    public Transform holdPoint;
    private GameObject heldItem;
    private bool isHoldingItem = false;

    private float interactCooldown = 0.25f;
    private float lastInteractTime = -1f;
    private Animator animator;

    void Awake()
    {
        if (interactableLayer == 0)
            interactableLayer = LayerMask.GetMask("Interactable");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInteractionInput();
    }

    void HandleInteractionInput()
    {
        if (Time.time - lastInteractTime < interactCooldown)
            return;
        if (Input.GetKeyDown(interactKey))
        {
            lastInteractTime = Time.time;
            // Eğer eşya tutuyorsa bırak, yoksa alma dene
            if (isHoldingItem)
                Drop();
            else
                TryInteract();
        }
    }

    void TryInteract()
    {
        Vector3 origin = transform.position + Vector3.up * interactionHeight + transform.forward * interactRange;
        Collider[] hits = Physics.OverlapBox(origin, boxSize * 0.5f, Quaternion.identity, interactableLayer);

        Debug.Log($"TryInteract: {hits.Length} collider(s) detected.");
        if (hits.Length == 0)
            return;

        foreach (Collider col in hits)
        {
            Debug.Log($"Checking collider: {col.name}, tag: {col.tag}");
            // Eğer elimizde yemek varsa ve etkileşime geçtiğimiz şey tabak ise
            if (isHoldingItem && col.CompareTag("Plate"))
            {
                Debug.Log("Elimde yemek var ve karşımdaki tabak.");
                Food heldFood = heldItem.GetComponent<Food>();
                Plate plate = col.GetComponent<Plate>();
                Debug.Log($"heldFood: {(heldFood != null ? heldFood.name : "null")}, plate: {(plate != null ? plate.name : "null")}");
                
                if (heldFood != null && plate != null && plate.CanAddFood(heldFood))
                {
                    Debug.Log("Yemek eklendi (AddFood çağrılıyor)");
                    plate.AddFood(heldFood);
                    heldItem = null;
                    isHoldingItem = false;
                    return;
                }
                else
                {
                    Debug.Log("Yemek eklenemedi: heldFood veya plate null ya da CanAddFood false");
                }
            }
            // Eğer elimizde tabak varsa ve etkileşime geçtiğimiz şey yemek ise
            else if (isHoldingItem && heldItem.CompareTag("Plate"))
            {
                Debug.Log("Elimde tabak var ve karşımdaki yemek.");
                Food foodOnGround = col.GetComponent<Food>();
                Plate heldPlate = heldItem.GetComponent<Plate>();
                Debug.Log($"foodOnGround: {(foodOnGround != null ? foodOnGround.name : "null")}, heldPlate: {(heldPlate != null ? heldPlate.name : "null")}");
                
                if (foodOnGround != null && heldPlate != null && heldPlate.CanAddFood(foodOnGround))
                {
                    Debug.Log("Tabaktayken yemek eklendi (AddFood çağrılıyor)");
                    heldPlate.AddFood(foodOnGround);
                    return;
                }
                else
                {
                    Debug.Log("Tabaktayken yemek eklenemedi: foodOnGround veya heldPlate null ya da CanAddFood false");
                }
            }
            // Normal etkileşim
            else
            {
                Debug.Log("Normal etkileşim deneniyor.");
                IInteractable interactable = col.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Debug.Log("IInteractable bulundu, Interact çağrılıyor.");
                    animator.SetTrigger("Interact");
                    interactable.Interact(this);
                    break;
                }
                else
                {
                    Debug.Log("IInteractable bulunamadı.");
                }
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

        // Eğer item tabak ise, direkt al (yemekleriyle birlikte)
        if (item.CompareTag("Plate"))
        {
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
            return;
        }

        // Eğer item Food ise ve tabağa kitlendiyse alma!
        Food food = item.GetComponent<Food>();
        if (food != null && food.isOnPlate)
        {
            Debug.Log("Bu yemek tabağa kitli, alınamaz!");
            return;
        }

        heldItem = item;
        isHoldingItem = true;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent(out Rigidbody rb2))
        {
            rb2.isKinematic = true;
            rb2.detectCollisions = false;
        }
    }

    public void Drop()
    {
        if (!isHoldingItem) return;

        GameObject itemToDrop = heldItem;
        heldItem = null;
        isHoldingItem = false;

        // DropZone tespiti
        Vector3 origin = transform.position + Vector3.up * interactionHeight + transform.forward * interactRange;
        Collider[] hits = Physics.OverlapBox(origin, boxSize * 0.5f, Quaternion.identity, interactableLayer);

        Transform dropParent = null;
        DropZone dropZone = null;
        foreach (Collider col in hits)
        {
            if (col.CompareTag("DropZone"))  // DropZone objelerine bu tag'ı ver!
            {
                dropZone = col.GetComponent<DropZone>();
                if (dropZone != null && !dropZone.IsOccupied)
                {
                    dropParent = col.transform;
                    break;
                }
            }
        }

        // Eğer bir DropZone varsa ve boşsa, oraya parent et
        if (dropParent != null)
        {
            itemToDrop.transform.SetParent(dropParent);
            itemToDrop.transform.localPosition = Vector3.zero;
            itemToDrop.transform.localRotation = Quaternion.identity;
            dropZone.SetItem(itemToDrop);

            if (itemToDrop.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }
        }
        
        // DropZone yoksa veya doluysa eşyayı elinde tut
        else
        {
            heldItem = itemToDrop;
            isHoldingItem = true;
        }
    }

    // ÇÖPKUTUSU İÇİN EKLENDİ
    public bool IsHoldingItem()
    {
        return isHoldingItem;
    }

    //  ÇÖPKUTUSU İÇİN EKLENDİ  
    public void TrashItem()
    {
        if (isHoldingItem && heldItem != null)
        {
            Debug.Log($"Trashing item: {heldItem.name}");
            
            // Objeyi yok et
            Destroy(heldItem);
            
            // Değişkenleri sıfırla
            heldItem = null;
            isHoldingItem = false;
            
            Debug.Log("Item successfully trashed!");
        }
    }

    public bool HasItem() => isHoldingItem;
    public GameObject GetHeldItem() => heldItem;

    // 🔍 Etkileşim alanını sahnede göster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + Vector3.up * interactionHeight + transform.forward * interactRange;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
