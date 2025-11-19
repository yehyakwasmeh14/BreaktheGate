using UnityEngine;
using UnityEngine.InputSystem;

public class KeypadInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer = -1;

    [Header("References")]
    public Camera playerCamera;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckForKeypadButton();
        }
    }

    void CheckForKeypadButton()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            
            if (distance <= interactionDistance)
            {
                KeypadKey key = hit.transform.GetComponent<KeypadKey>();
                if (key != null)
                {
                    key.SendKey();
                }
            }
        }
    }
}
