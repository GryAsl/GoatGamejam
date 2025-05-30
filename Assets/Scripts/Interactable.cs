using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public string interactText = "Press [E] to interact";
    public bool oneTimeUse = false;
    public bool isEnabled = true;

    private bool hasBeenUsed = false;

    // Bu fonksiyon interface tarafından çağrılır
    public void Interact(PlayerInteraction player)
    {
        if (!isEnabled || (oneTimeUse && hasBeenUsed)) return;

        OnInteract(player);
        hasBeenUsed = true;
    }

    // Türev sınıflarda override edilecek asıl işlev
    protected abstract void OnInteract(PlayerInteraction player);

    // Opsiyonel: Oyuncuya bilgi göstermek için
    public virtual string GetInteractText()
    {
        return interactText;
    }
}