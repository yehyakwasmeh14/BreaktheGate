using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public string itemName = "Fuel";
    public string pickupPrompt = "Press R to pick up Fuel";

    public string GetPickupPrompt()
    {
        return pickupPrompt;
    }
}
