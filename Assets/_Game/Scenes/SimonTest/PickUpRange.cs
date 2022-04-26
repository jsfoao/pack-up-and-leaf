using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRange : MonoBehaviour
{
    private float radius;
    public float walkRadius;
    public float ballRadius;
    private PlayerManager _playerManager;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position,radius);
    }

    private void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    void FixedUpdate()
    {
        // Changes pick up radius based on PlayerManager.State

        if(_playerManager.state==PlayerManager.State.Rolling)
        {
            radius = ballRadius;
        }
        else if (_playerManager.state==PlayerManager.State.Normal)
        {
            radius = walkRadius;
        }
        Interact();


    }



    void Interact()
    {
        //Detecting Interactive layers
        Collider[] Zone = Physics.OverlapSphere(gameObject.transform.position, radius, LayerMask.GetMask("Interactive"));

        foreach (var Interactableobject in Zone)
        {

            //Make decisions on objects based on their tags

            if (Interactableobject.tag == "Leaf")
            {
                Interactableobject.GetComponent<LerpToPlayer>().StartLerp();
            }
            else if (Interactableobject.tag == "Berry")
            {
                Interactableobject.GetComponent<LerpToPlayer>().StartLerp();
            }


        }

    }




}