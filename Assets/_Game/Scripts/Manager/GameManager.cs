using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerEntity entity;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onAwake;
    

    private void Start()
    {
        onStart.Invoke();
    }

    private void Awake()
    {
        onAwake.Invoke();
    }
}
