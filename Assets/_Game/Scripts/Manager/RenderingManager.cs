using System;
using UnityEngine;

public class RenderingManager : MonoBehaviour
{
    [SerializeField] private RenderEntity[] renderEntity;
    
    public void EnableRenderer(string name)
    {
        foreach (RenderEntity entity in renderEntity)
        {
            if (entity.Name == name)
            {
                entity.Renderer.enabled = true;
            }
        }
    }
    
    public void DisableRenderer(string name)
    {
        foreach (RenderEntity entity in renderEntity)
        {
            if (entity.Name == name)
            {
                entity.Renderer.enabled = false;
            }
        }
    }
}

[Serializable]
public class RenderEntity
{
    public string Name;
    public Renderer Renderer;
}
