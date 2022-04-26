using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreeCamera : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<CameraTarget> targets;

    private CameraTarget currentTarget;
    [SerializeField, Range(1f, 100f)] private float moveSpeed;
    [SerializeField, Range(0.1f, 5f)] private float rotationSpeed;
    [SerializeField] private float positionThreshold;
    [SerializeField] private float rotationThreshold;
    
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Motion motionState;
    [SerializeField] private float _currentTime;

    [SerializeField] private UnityEvent onAnimationEnd;
    public bool active;


    public void SetActive()
    {
        active = true;
    }
    
    private bool ComparePosition(Vector3 posA, Vector3 posB)
    {
        if (Mathf.Abs(posA.magnitude - posB.magnitude) <= positionThreshold)
        {
            return true;
        }
        return false;
    }

    private bool CompareAngle(Quaternion quatA, Quaternion quatB)
    {
        float angle = Quaternion.Angle(quatA, quatB);
        if (Mathf.Abs(angle) <= rotationThreshold)
        {
            return true;
        }
        return false;
    }
    
    private void Update()
    {
        if (!active) { return; }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            onAnimationEnd.Invoke();
        }
        
        switch (motionState)
        {
            case Motion.Done:
                break;
            
            case Motion.Waiting:
                WaitTimer();
                break;
            
            case Motion.Moving:
                // Movement
                transform.position = Vector3.SmoothDamp(transform.position, currentTarget.transform.position, ref velocity, moveSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentTarget.transform.rotation, rotationSpeed * Time.deltaTime);

                if (ComparePosition(transform.position, currentTarget.transform.position) && CompareAngle(transform.rotation, currentTarget.transform.rotation))
                {
                    if (currentTarget.Next != null)
                    {
                        motionState = Motion.Waiting;
                    }
                    else
                    {
                        motionState = Motion.Done;
                        onAnimationEnd.Invoke();
                    }
                }
                break;
        }
    }
    
    private void WaitTimer()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            if (currentTarget.Next != null)
            {
                currentTarget = currentTarget.Next;
                _currentTime = currentTarget.WaitTime;
                motionState = Motion.Moving;
            }
        }
    }
    
    private void Start()
    {
        active = false;
        motionState = Motion.Waiting;
        if (targets.Count != 0)
        {
            currentTarget = targets[0];
        }
        transform.position = targets[0].transform.position;
        transform.rotation = targets[0].transform.rotation;
        _currentTime = targets[0].WaitTime;
    }
}

public enum Motion
{
    Moving, Waiting, Done
}
