using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerEntity entity;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onAwake;

    public static GameManager Instance;
    

    private void Start()
    {
        entity = FindObjectOfType<PlayerEntity>();
        if (entity == null)
        {
            Debug.LogWarning("No player entity in world");
        }
        else
        {
            Debug.Log("Found entity");
        }
        
        if (GetPlayerEntity() != null)
        {
            SetEntityInput(true);
        }
        onStart.Invoke();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        onAwake.Invoke();
    }

    public PlayerEntity GetPlayerEntity()
    {
        if (entity == null)
        {
            Debug.LogError("Couldn't get entity. Entity is null");
        }
        return entity;
    }

    public void SetEntityInput(bool active)
    {
        InputManager inputManager = GetPlayerEntity().GetComponent<InputManager>();
        inputManager.SetInput(active);
    }
}
