using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string dropPrompt = "Press R to drop Fuel";
    public string acceptedItemTag = "Pickupable";

    private PlayerPickupSystem playerInZone;

    public string GetDropPrompt()
    {
        return dropPrompt;
    }

    public bool AcceptsItem(string itemTag)
    {
        return itemTag == acceptedItemTag;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerPickupSystem pickupSystem = GetPickupSystem(other);
        if (pickupSystem != null && playerInZone == null)
        {
            playerInZone = pickupSystem;
            pickupSystem.EnterDropZone(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerPickupSystem pickupSystem = GetPickupSystem(other);
        if (pickupSystem != null && pickupSystem == playerInZone)
        {
            playerInZone = null;
            pickupSystem.ExitDropZone(this);
        }
    }

    void OnTriggerStay(Collider other)
    {
        PlayerPickupSystem pickupSystem = GetPickupSystem(other);
        if (pickupSystem != null && playerInZone == null)
        {
            playerInZone = pickupSystem;
            pickupSystem.EnterDropZone(this);
        }
    }

    PlayerPickupSystem GetPickupSystem(Collider other)
    {
        PlayerPickupSystem pickupSystem = other.GetComponentInParent<PlayerPickupSystem>();
        if (pickupSystem == null)
        {
            pickupSystem = other.GetComponent<PlayerPickupSystem>();
        }
        return pickupSystem;
    }
}
