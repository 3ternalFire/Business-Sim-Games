using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Transform spawnPoint;               // Where to spawn
    public List<GameObject> spawnPrefabs;      // Prefabs to choose from

    private GameObject currentSpawned;         // Tracks currently spawned object

    void Update()
    {
        // If no object is currently spawned, spawn a new one
        if (currentSpawned == null && spawnPrefabs.Count > 0)
        {
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        // Pick a random prefab
        int randomIndex = Random.Range(0, spawnPrefabs.Count);
        GameObject prefab = spawnPrefabs[randomIndex];

        // Spawn it at the spawn point
        currentSpawned = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
