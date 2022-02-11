using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefSetter : MonoBehaviour
{
    public Vector3Var playerPosition;


    private void Update()
    {
        playerPosition.Value = transform.position;
    }
}
