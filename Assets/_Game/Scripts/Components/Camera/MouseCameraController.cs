using System;
using UnityEngine;

[RequireComponent(typeof(OrbitCamera))]
public class MouseCameraController : MonoBehaviour
{
    private OrbitCamera _orbitCamera;
    [NonSerialized] public Vector2 mouseInput;
    
    [Header("Sensitivity")] 
    [SerializeField, Range(0.001f, 0.2f), Tooltip("How much to move mouse until input is read")] private float deadzone = 0.05f;
    [SerializeField, Range(0.1f, 20f), Tooltip("Horizontal speed")] private float sensitivityX;
    [SerializeField, Range(0.1f, 20f), Tooltip("Vertical speed")] private float sensitivityY;
    [SerializeField, Range(0.1f, 10f), Tooltip("Scroll speed")] private float sensitivityScroll;
    
    [Header("Scrolling")] 
    [SerializeField] private bool scrollEnable;
    [SerializeField, Range(1f, 20f)] private float minScroll;
    [SerializeField, Range(1f, 20f)] private float maxScroll;

    
    [Header("Axis")]
    [SerializeField, Tooltip("Invert Horizontal axis")] private bool inverseX;
    [SerializeField, Tooltip("Invert Vertical axis")] private bool inverseY;
    [SerializeField, Tooltip("Invert Scroll axis")] private bool inverseScroll;
    
    [SerializeField] private float inputTime;
    private float currentTime;
    private bool timer;
    
    // UI
    [SerializeField] private float sensMultiplier;
    
    private OptionMenuUI _optionMenuUI;
    
    private void Update()
    {
        SetUI();
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        // Inverse axis
        int multiplierX = inverseX ? -1 : 1;
        int multiplierY = inverseY ? 1 : -1;
        int multiplierScroll = inverseScroll ? 1 : -1;

        if (scrollEnable)
        {
            _orbitCamera.Radius += multiplierScroll * Input.GetAxis("Mouse ScrollWheel") * sensitivityScroll;
        }
        if (mouseInput.x < -deadzone ||mouseInput.x > deadzone || mouseInput.y < -deadzone || mouseInput.y > deadzone)
        {
            _orbitCamera.cameraState = CameraState.Orbit;
            _orbitCamera.Angles += new Vector2(multiplierX * mouseInput.x * sensitivityX, multiplierY * mouseInput.y * sensitivityY);
            
            currentTime = inputTime;
            timer = true;
        }
        else
        {
            RunTimer(inputTime);
        }
    }

    private void RunTimer(float time)
    {
        if (timer)
        {
            // Reset input timer
            currentTime -= Time.unscaledDeltaTime;
            if (currentTime <= 0)
            {
                currentTime = time;
                _orbitCamera.cameraState = CameraState.Follow;
                timer = false;
            }
        }
    }

    private void SetUI()
    {
        #region UI
        if (_optionMenuUI != null)
        {
            if (_optionMenuUI.ReturnMouseValue() > 0f)
            {
                SetSensitivity(_optionMenuUI.ReturnMouseValue() * sensMultiplier);
            }
            inverseX = _optionMenuUI.ReturnMouseBool();
        }
        #endregion
    }
    
    public void SetSensitivity(float xSens, float ySens)
    {
        sensitivityX = xSens;
        sensitivityY = ySens;
    }
    
    public void SetSensitivity(float sens)
    {
        sensitivityX = sens;
        sensitivityY = sens;
    }

    private void Start()
    {
        _orbitCamera = GetComponent<OrbitCamera>();
        _optionMenuUI = FindObjectOfType<OptionMenuUI>();
        if (_optionMenuUI == null)
        {
            SetSensitivity(2f);
        }
        else
        {
            SetSensitivity(0.5f * sensMultiplier);
        }
    }

    private void OnValidate()
    {
        if (minScroll > maxScroll)
        {
            minScroll = maxScroll;
        }
    }
}
