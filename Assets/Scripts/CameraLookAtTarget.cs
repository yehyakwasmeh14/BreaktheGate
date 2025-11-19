using UnityEngine;

public class CameraLookAtTarget : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform targetCharacter;
    public float heightOffset = 1.5f;
    public float positionSmoothTime = 0.1f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (targetCharacter == null) return;

        Vector3 targetPosition = targetCharacter.position + Vector3.up * heightOffset;
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            positionSmoothTime
        );
        
        transform.rotation = Quaternion.identity;
    }
}
