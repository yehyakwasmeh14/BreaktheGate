using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerPickupSystem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer = -1;

    [Header("Carry Settings")]
    public Transform holdPosition;
    public Vector3 holdOffset = new Vector3(0, 1, 1.5f);

    [Header("UI")]
    public Text interactionText;

    [Header("Audio")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    public float audioVolume = 0.7f;

    [Header("References")]
    public Camera playerCamera;

    private GameObject carriedObject;
    private Rigidbody carriedRigidbody;
    private PickupableItem currentPickupable;
    private DropZone currentDropZone;
    private AudioSource audioSource;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (holdPosition == null)
        {
            GameObject holdPoint = new GameObject("HoldPosition");
            holdPoint.transform.parent = playerCamera.transform;
            holdPoint.transform.localPosition = holdOffset;
            holdPosition = holdPoint.transform;
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
        audioSource.volume = audioVolume;
    }

    void Update()
    {
        if (carriedObject == null)
        {
            CheckForPickupable();
        }
        else
        {
            UpdateCarriedObject();
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            HandleInteraction();
        }
    }

    void CheckForPickupable()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            PickupableItem pickupable = hit.collider.GetComponent<PickupableItem>();
            
            if (pickupable != null)
            {
                currentPickupable = pickupable;
                ShowPrompt(pickupable.GetPickupPrompt());
                return;
            }
        }

        currentPickupable = null;
        HidePrompt();
    }

    public void EnterDropZone(DropZone zone)
    {
        if (carriedObject != null && zone.AcceptsItem(carriedObject.tag))
        {
            currentDropZone = zone;
            ShowPrompt(zone.GetDropPrompt());
        }
    }

    public void ExitDropZone(DropZone zone)
    {
        if (currentDropZone == zone)
        {
            currentDropZone = null;
            HidePrompt();
        }
    }

    void HandleInteraction()
    {
        if (carriedObject == null && currentPickupable != null)
        {
            PickupObject(currentPickupable.gameObject);
        }
        else if (carriedObject != null && currentDropZone != null)
        {
            DropObject();
        }
    }

    void PickupObject(GameObject obj)
    {
        carriedObject = obj;
        carriedRigidbody = obj.GetComponent<Rigidbody>();

        if (carriedRigidbody != null)
        {
            carriedRigidbody.isKinematic = true;
            carriedRigidbody.useGravity = false;
        }

        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;
        }

        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        currentPickupable = null;
        HidePrompt();
    }

    void DropObject()
    {
        if (carriedObject == null || currentDropZone == null)
        {
            return;
        }

        Vector3 dropPosition = currentDropZone.transform.position;
        
        RaycastHit groundHit;
        if (Physics.Raycast(currentDropZone.transform.position, Vector3.down, out groundHit, 100f))
        {
            Collider fuelCollider = carriedObject.GetComponent<Collider>();
            float objectHeight = 0f;
            
            if (fuelCollider != null)
            {
                objectHeight = fuelCollider.bounds.extents.y;
            }
            
            dropPosition = groundHit.point;
            dropPosition.y += objectHeight;
        }
        
        carriedObject.transform.position = dropPosition;
        carriedObject.transform.rotation = Quaternion.Euler(270, 0, 0);

        Collider objCollider = carriedObject.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = true;
        }

        if (carriedRigidbody != null)
        {
            carriedRigidbody.linearVelocity = Vector3.zero;
            carriedRigidbody.angularVelocity = Vector3.zero;
            carriedRigidbody.isKinematic = true;
            carriedRigidbody.useGravity = false;
            carriedRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        if (audioSource != null && dropSound != null)
        {
            AudioSource.PlayClipAtPoint(dropSound, dropPosition, audioVolume);
        }

        ExplosiveFuel explosiveCheck = carriedObject.GetComponent<ExplosiveFuel>();
        if (explosiveCheck != null)
        {
            explosiveCheck.Arm();
        }

        if (currentDropZone != null)
        {
            Destroy(currentDropZone.gameObject);
        }

        carriedObject = null;
        carriedRigidbody = null;
        currentDropZone = null;
        HidePrompt();
    }

    void UpdateCarriedObject()
    {
        if (carriedObject != null && holdPosition != null)
        {
            carriedObject.transform.position = holdPosition.position;
            carriedObject.transform.rotation = holdPosition.rotation;
        }
    }

    void ShowPrompt(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
        }
    }

    void HidePrompt()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
