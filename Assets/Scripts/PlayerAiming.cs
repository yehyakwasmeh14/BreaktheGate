using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerAiming : MonoBehaviour
{
    [Header("References")]
    public CinemachineCamera cinemachineCamera;
    
    [Header("Zoom Settings")]
    public float normalFOV = 40f;
    public float aimFOV = 30f;
    public float zoomSpeed = 10f;
    
    [Header("Screen Offset")]
    public Vector3 aimCameraOffset = new Vector3(0.6f, 0.2f, 0f);
    public float offsetSpeed = 8f;
    
    private Animator animator;
    private float targetFOV;
    private CinemachineCameraOffset cameraOffset;
    private Vector3 normalOffset = Vector3.zero;
    
    private const string AIMING_PARAM = "Aiming";
    
    public bool IsAiming { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }
        
        if (cinemachineCamera != null)
        {
            normalFOV = cinemachineCamera.Lens.FieldOfView;
            targetFOV = normalFOV;
            
            cameraOffset = cinemachineCamera.GetComponent<CinemachineCameraOffset>();
            if (cameraOffset != null)
            {
                normalOffset = cameraOffset.Offset;
            }
        }
    }

    void Update()
    {
        if (Mouse.current != null && animator != null)
        {
            IsAiming = Mouse.current.rightButton.isPressed;
            animator.SetBool(AIMING_PARAM, IsAiming);
            
            targetFOV = IsAiming ? aimFOV : normalFOV;
        }
        
        if (cinemachineCamera != null)
        {
            var lens = cinemachineCamera.Lens;
            lens.FieldOfView = Mathf.Lerp(lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
            cinemachineCamera.Lens = lens;
        }
        
        if (cameraOffset != null)
        {
            Vector3 targetOffset = IsAiming ? aimCameraOffset : normalOffset;
            cameraOffset.Offset = Vector3.Lerp(cameraOffset.Offset, targetOffset, Time.deltaTime * offsetSpeed);
        }
    }
}

