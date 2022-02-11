using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(OnCollisionPlayer))]
public class Interactable : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)] private float range;
    protected bool inRange;
    [SerializeField] private UnityEvent onInteract;

    [SerializeField, Tooltip("Key that trigger interaction")]
    private KeyCode key;

    [SerializeField] private float outlineWidth;
    [SerializeField] private MeshRenderer[] renderers;
    [NonSerialized] public bool active;


    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (!inRange || !active)
            {
                return;
            }

            onInteract.Invoke();
        }
    }

    public void InRange()
    {
        var mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_OutlineWidth", outlineWidth);
        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(mpb);
        }

        inRange = true;
    }

    public void OutRange()
    {
        var mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_OutlineWidth", 0f);
        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(mpb);
        }

        inRange = false;
    }

    public void Disable()
    {
        outlineWidth = 0f;
        var mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_OutlineWidth", 0f);
        foreach (var renderer in renderers)
        {
            renderer.SetPropertyBlock(mpb);
        }

        active = false;
    }

    private void Start()
    {
        active = true;
    }

    private void OnValidate()
    {
        GetComponent<SphereCollider>().radius = range;
    }

    private void OnDrawGizmos()
    {
        if (inRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}