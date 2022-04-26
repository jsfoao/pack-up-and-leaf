using System;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    [SerializeField] private float speedKeyPan;
    [SerializeField] private float speedMousePan;
    [SerializeField] private float speedRotation;

    private float xRotation;
    private float yRotation;

    private CameraManager _cameraManager;
    private void Move(Vector3 direction, float speed)
    {
        transform.position += direction * (speed * Time.deltaTime);
    }

    private void DoKeyMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(-transform.right, speedKeyPan);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(transform.right, speedKeyPan);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(-transform.forward, speedKeyPan);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(transform.forward, speedKeyPan);
        }
    }

    private void DoMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 directionX = transform.right * -mouseX;
        Vector3 directionY = transform.up * -mouseY;
        
        Move(directionX + directionY, speedMousePan);
    }

    private void DoMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * speedRotation;
        float mouseY = Input.GetAxis("Mouse Y") * speedRotation;

        xRotation += mouseY;
        yRotation -= mouseX;

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0f) * -1;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _cameraManager.ActivateCamera("CineCam");
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {
            DoMouseMovement();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            DoMouseRotation();
        }
        DoKeyMovement();
    }

    private void Start()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
    }
}
