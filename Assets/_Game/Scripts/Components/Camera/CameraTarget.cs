using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public CameraTarget Next;
    public float WaitTime;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
        
        Gizmos.color = Color.white;
        if (Next == null) { return; }
        Gizmos.DrawLine(transform.position, Next.transform.position);
    }
}
