using UnityEngine;

public class DisableRendering : MonoBehaviour
{
    public void Disable(Collider collider)
    {
        Rendering rendering = collider.GetComponent<Rendering>();
        if (rendering == null) { return; }

        rendering.Renderer.enabled = false;
    }

    public void Enable(Collider collider)
    {
        Rendering rendering = collider.GetComponent<Rendering>();
        if (rendering == null) { return; }

        rendering.Renderer.enabled = true;
    }
}