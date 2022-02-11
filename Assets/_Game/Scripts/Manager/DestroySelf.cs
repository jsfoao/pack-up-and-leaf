using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
