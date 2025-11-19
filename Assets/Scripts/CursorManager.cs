using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Lock Settings")]
    public bool lockOnStart = true;

    private bool cursorLocked = false;

    void Start()
    {
        if (lockOnStart)
        {
            LockCursor();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }
        
        if (!cursorLocked && Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
            {
                LockCursor();
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && lockOnStart && cursorLocked)
        {
            LockCursor();
        }
    }
}

