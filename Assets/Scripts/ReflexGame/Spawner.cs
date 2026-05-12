using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public GameObject shellPrefab;
    public float spawnRate = 1.5f;
    public float xRange = 4f;

    void Start()
    {
        InvokeRepeating("SpawnLogic", 0.5f, spawnRate);
    }

    void SpawnLogic()
    {
        GameObject prefabToSpawn;

        if (Random.value < 0.3f)
        {
            prefabToSpawn = shellPrefab;
        }
        else
        {
            prefabToSpawn = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        }

        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}