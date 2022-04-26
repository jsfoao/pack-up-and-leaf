using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSetter : MonoBehaviour, IZoneInteraction
{
    [SerializeField] Transform respawnObjTransform;
    Vector3 respawnPosition;
    public void OnInteract(Transform interacter)
    {
        Debug.Log(interacter.position);
        IEntity entity = interacter.GetComponent<IEntity>();
        entity.spawnPoint = respawnPosition;
    }

    private void Start()
    {
        respawnPosition = respawnObjTransform.position;
    }
}
