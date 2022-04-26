using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject _plane;
    private Vector3 planeMeassures;
    private Vector3 StartMeassures;
    private float boundsX;
    private float boundsY;
    private float boundsZ;
   [SerializeField] private GameObject prefab;
    void Start()
    { 
        Mesh planeMesh = _plane.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        boundsX = _plane.transform.localScale.x * bounds.size.x; 
        boundsY = _plane.transform.localScale.y * bounds.size.y;
        boundsZ = _plane.transform.localScale.z * bounds.size.z;
        Debug.Log(boundsZ+" Z "+ boundsY +" Y "+ boundsX +" X ");
        Spawn();
    }
    

    void Spawn()
    {
        planeMeassures = new Vector3(boundsX, boundsY, boundsZ);

        for (int i = 0; i < 5; i++)
        {
            int xcount = Random.Range(-10, 10);
                    int zcount = Random.Range(-10, 10);
                     StartMeassures = new Vector3(xcount, 0, zcount);
                    Instantiate(prefab, StartMeassures, quaternion.identity);
        }
        
    }
}
