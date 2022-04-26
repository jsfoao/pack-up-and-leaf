using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform travelTransform;
    [SerializeField] float timeToTravel;
    Rigidbody rb;

    Vector3 travelDestination;
    Vector3 startDestination;
    float currentTime;
    
    enum Status { TravellingTo, TravellingBack};
    Status status = new Status();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        startDestination = transform.position;
        travelDestination = travelTransform.position;
        
        status = Status.TravellingTo;
    }

    private void FixedUpdate()
    {
        Vector3 pos;
        if (status == Status.TravellingTo)
        {
            currentTime += Time.fixedDeltaTime;
            float percentage = currentTime / timeToTravel;
            pos = Vector3.Lerp(startDestination, travelDestination, percentage);
            if (percentage > 1)
            {
                status = Status.TravellingBack;
                currentTime = 0;
            }
        }
        else
        {
            currentTime += Time.fixedDeltaTime;
            float percentage = currentTime / timeToTravel;
            pos = Vector3.Lerp(travelDestination, startDestination, percentage);
            if (percentage > 1)
            {
                status = Status.TravellingTo;
                currentTime = 0;
            }
        }
        //
        rb.MovePosition(pos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(travelTransform.position, transform.position);
    }


}
