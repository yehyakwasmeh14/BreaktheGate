using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("References")]
    public Transform cameraTransform;
    public PlayerAiming playerAiming;
    
    [Header("Animation")]
    public Animator animator;
    public float animationSmoothTime = 0.1f;

    private CharacterController characterController;
    private Vector2 movementInput;
    private Vector3 moveDirection;
    private float currentAnimX;
    private float currentAnimY;
    private float animXVelocity;
    private float animYVelocity;
    private float verticalVelocity;

    private const float INPUT_THRESHOLD = 0.01f;
    private const float GRAVITY = -9.81f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        if (playerAiming == null)
        {
            playerAiming = GetComponent<PlayerAiming>();
        }
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        moveDirection = CalculateCameraRelativeMovement();
        
        Vector3 movement = moveDirection * moveSpeed;
        
        ApplyGravity();
        movement.y = verticalVelocity;
        
        characterController.Move(movement * Time.deltaTime);
    }

    Vector3 CalculateCameraRelativeMovement()
    {
        if (movementInput.sqrMagnitude < INPUT_THRESHOLD)
        {
            return Vector3.zero;
        }
        
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        if (cameraTransform == null)
        {
            return new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = cameraForward * movementInput.y + cameraRight * movementInput.x;
        
        return desiredMoveDirection.normalized;
    }

    void HandleRotation()
    {
        if (playerAiming == null)
        {
            playerAiming = GetComponent<PlayerAiming>();
        }
        
        if (playerAiming != null && playerAiming.IsAiming)
        {
            if (cameraTransform != null)
            {
                Vector3 lookDirection = cameraTransform.forward;
                lookDirection.y = 0f;
                
                if (lookDirection.sqrMagnitude > 0.01f)
                {
                    lookDirection.Normalize();
                    Quaternion aimRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, 
                        aimRotation, 
                        rotationSpeed * 2f * Time.deltaTime
                    );
                }
            }
            return;
        }
        
        if (moveDirection.sqrMagnitude < INPUT_THRESHOLD)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
        }
        else
        {
            verticalVelocity += GRAVITY * Time.deltaTime;
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        Vector2 animatorInput = Vector2.zero;
        
        if (moveDirection.sqrMagnitude > INPUT_THRESHOLD)
        {
            Vector3 localMove = transform.InverseTransformDirection(moveDirection);
            animatorInput = new Vector2(localMove.x, localMove.z);
        }

        currentAnimX = Mathf.SmoothDamp(
            currentAnimX, 
            animatorInput.x, 
            ref animXVelocity, 
            animationSmoothTime
        );
        
        currentAnimY = Mathf.SmoothDamp(
            currentAnimY, 
            animatorInput.y, 
            ref animYVelocity, 
            animationSmoothTime
        );

        animator.SetFloat("InputX", currentAnimX);
        animator.SetFloat("InputY", currentAnimY);
    }
}
