using System;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private bool isPaused;
    private bool runTimer;
    private float mouseInputTimer = 1f;

    private bool mouseDisabled;
    

    public void SetPaused()
    {
        isPaused = true;
    }

    public void SetUnpaused()
    {
        isPaused = false;
    }

    private void ActivateMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DeactivateMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseDisabled = true;
    }

    private void ExecuteMouseBuffer()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        if (mouseInput.magnitude > 0f)
        {
            ActivateMouseCursor();
            mouseInputTimer = 1f;
            runTimer = false;
            mouseDisabled = false;
            Debug.Log("this");
        }
        else
        {
            runTimer = true;
        }

        if (runTimer)
        {
            mouseInputTimer -= Time.unscaledDeltaTime;
        }

        mouseInputTimer = Mathf.Clamp(mouseInputTimer, -1, 1f);
        if (mouseInputTimer <= 0f)
        {
            if (mouseDisabled) { return; }

            runTimer = false;
            DeactivateMouseCursor();
        }
    }
    
    
    private void Update()
    {
        // Disabling mouse input timer
        if (!isPaused) { return; }
        ExecuteMouseBuffer();
    }
    
    private void Start()
    {
        mouseInputTimer = 1f;
        mouseDisabled = false;
    }
}
