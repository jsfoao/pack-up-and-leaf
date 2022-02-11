using System;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private Camera _camera;
    private OrbitCamera _orbitCamera;
    [SerializeField] private Vector2 startAngle;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        _orbitCamera.Angles += new Vector2(1, 0) * rotationSpeed * Time.deltaTime;
        if (_camera.enabled == false)
        {
            _orbitCamera.Angles = startAngle;
        }
    }

    private void Start()
    {
        _orbitCamera = GetComponent<OrbitCamera>();
        _camera = GetComponent<Camera>();
    }
}
