using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private SpeedState speedState;
    [Header("Normal State")]
    [SerializeField, Range(1, 20f)] private float defaultRadius;
    [SerializeField, Range(1f, 120f)] private float defaultFov;

    [Header("Supersonic State")]
    [SerializeField, Range(1, 20f)] private float supersonicRadius;
    [SerializeField, Range(1f, 120f)] private float supersonicFov;

    [Header("Transition")]
    [SerializeField, Tooltip("Velocity at which it'll trigger transition")]
    private float velocity;
    [SerializeField, Range(0.1f, 20f)] private float transitionSpeed;

    [SerializeField] private GameObject player;
    private PlayerManager _playerManager;
    private Rigidbody _playerRb;
    private Camera _camera;
    private OrbitCamera _orbitCamera;
    private float _radius;
    private float _fov;

    private void ExecuteNormal()
    {
        _radius = defaultRadius;
        _orbitCamera.LerpRadius(_radius, transitionSpeed * Time.unscaledDeltaTime);
        _fov = Mathf.Lerp(_fov, defaultFov, transitionSpeed * Time.unscaledDeltaTime);
    }

    private void ExecuteSupersonic()
    {
        _radius = supersonicRadius;
        _orbitCamera.LerpRadius(_radius, transitionSpeed * Time.unscaledDeltaTime);
        _fov = Mathf.Lerp(_fov, supersonicFov, transitionSpeed * Time.unscaledDeltaTime);
    }
    
    private void Update()
    {
        if (_playerRb == null) { return; }
        float playerVelocity = _playerRb.velocity.magnitude;
        
        switch (speedState)
        {
            case SpeedState.Normal:
                ExecuteNormal();
                if (playerVelocity > velocity && _playerManager.state == PlayerManager.State.Rolling)
                {
                    speedState = SpeedState.Supersonic;
                }
                break;
            case SpeedState.Supersonic:
                ExecuteSupersonic();
                if (playerVelocity <= velocity)
                {
                    speedState = SpeedState.Normal;
                }
                break;
        }

        _camera.fieldOfView = _fov;
    }

    private void Start()
    {
        if (player == null)
        {
            return;
        }
        _playerManager = player.GetComponent<PlayerManager>();
        _playerRb = _playerManager.GetComponent<Rigidbody>();
        _camera = GetComponent<Camera>();
        _orbitCamera = GetComponent<OrbitCamera>();
    }
}

public enum SpeedState
{
    Normal, Supersonic
}
