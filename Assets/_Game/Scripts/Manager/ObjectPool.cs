using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;

    private Queue<GameObject> freeGameObjectsPool; // can be Stack, or whatever else

    private void WarmUp()
    {
        freeGameObjectsPool = new Queue<GameObject>();

        // this is optional, but a good idea
        for (var i = 0; i < 1000; ++i)
        {
            var go = Instantiate(prefab);
            go.SetActive(false);
            freeGameObjectsPool.Enqueue(go);
        }
    }

    public GameObject Spawn()
    {
        // if (freeGameObjectsPool.TryDequeue(out var result))
        // {
        //     result.SetActive(true);
        //     return result;
        // }

        // of we can return null if our pool is "fixed" size
        return Instantiate(prefab);
    }

    public void ObjectPoolReturn(GameObject go)
    {
        go.SetActive(false);
        freeGameObjectsPool.Enqueue(go);
    }
    
    private void Start()
    {
        WarmUp();
    }
}