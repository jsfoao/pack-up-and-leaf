using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private bool active;
    
    private float _currentTime;
    public UnityEvent onTimerEnd;
    public UnityEvent onTimerStart;
    
    public void Enable()
    {
        active = true;
        onTimerStart.Invoke();
    }

    public void Disable()
    {
        active = false;
        onTimerEnd.Invoke();
    }

    private void Update()
    {
        if (active)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                _currentTime = time;
                Disable();
            }
        }
    }

    private void OnValidate()
    {
        _currentTime = time;
    }
}
