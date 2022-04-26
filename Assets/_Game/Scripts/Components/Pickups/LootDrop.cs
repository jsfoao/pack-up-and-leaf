using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private float spawnOffset;
    [SerializeField] private Loot[] loots;
    [SerializeField] private Range<float> distanceRange;
    [SerializeField] private Range<int> lootRange;
    
    public void DropRandom()
    {
        int lootToSpawn = Random.Range(lootRange.Min, lootRange.Max);
        for (int i = 0; i < lootToSpawn; i++)
        {
            float size = Random.Range(distanceRange.Min, distanceRange.Max);
            
            float angle = i * (2 * Mathf.PI / lootToSpawn);
            float xPos = Mathf.Cos(angle);
            float zPos = Mathf.Sin(angle);
            
            Vector3 position = transform.position + new Vector3(xPos, 0f, zPos) * size + Vector3.up * spawnOffset;

            if (loots.Length == 0) { return; }
            int index = RandomIndex();
            GameObject instance = Instantiate(loots[index].Prefab, position, Quaternion.identity);
        }
    }

    private int RandomIndex()
    {
        float randomFloat = Random.Range(0f, 1f);
        Debug.Log(randomFloat);
        int index = 0;
        for (int i = 0; i < loots.Length; i++)
        {
            if (randomFloat >= loots[i].minChance && randomFloat <= loots[i].maxChance)
            {
                return i;
            }
        }
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Vector3 gizmoPos = new Vector3(transform.position.x, transform.position.y + spawnOffset, transform.position.z);
        Gizmos.DrawWireSphere(gizmoPos, distanceRange.Min);
        Gizmos.DrawWireSphere(gizmoPos, distanceRange.Max);

        Gizmos.color = Color.red;
        for (int i = 0; i < lootRange.Max; i++)
        {
            float size = distanceRange.Max;
            
            float angle = i * (2 * Mathf.PI / lootRange.Max);
            float xPos = Mathf.Cos(angle);
            float zPos = Mathf.Sin(angle);
            
            Vector3 position = transform.position + new Vector3(xPos, 0f, zPos) * size + Vector3.up * spawnOffset;
            Gizmos.DrawLine(gizmoPos, position);
        }
    }
}

[Serializable]
public struct Range<T>
{
    [Range(0, 20)] public T Min;
    [Range(0, 20)] public T Max;
}

[Serializable]
public class Loot
{
    public GameObject Prefab;
    [Range(0f, 1f)] public float minChance;
    [Range(0f, 1f)] public float maxChance;
}