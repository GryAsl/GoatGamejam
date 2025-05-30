using UnityEngine;

public class PickableObject : Interactable
{
    [Header("Optional Settings")]
    public bool canBePickedUp = true;

    protected override void OnInteract(PlayerInteraction player)
    {
        if (!canBePickedUp)
        {
            Debug.Log($"{name} cannot be picked up.");
            return;
        }

        if (!player.HasItem())
        {
            player.PickUp(gameObject);
        }
        else
        {
            Debug.Log("Player is already holding something.");
        }
    }

    public override string GetInteractText()
    {
        return canBePickedUp ? "Press [E] to Pick Up" : "";
    }
}