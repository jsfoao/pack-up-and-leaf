using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    private IZoneInteraction zoneInteraction;

    private void OnTriggerEnter(Collider other)
    {
        zoneInteraction.OnInteract(other.transform);
    }

    private void Awake()
    {
        zoneInteraction = GetComponent<IZoneInteraction>();
    }

}
