using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _components;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _gameObject;
    
    

    public void DisableComponents()
    {
        foreach (var component in _components)
        {
            component.enabled = false;
        }
        _collider.enabled = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _renderer.enabled = false;
        Destroy(_gameObject);
    }
}
