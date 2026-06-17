using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public GameObject shellPrefab;

    [Header("Réglages de Vitesse")]
    public float spawnRate = 1.5f;
    public float minSpawnRate = 0.4f;
    public float difficultyStep = 0.15f;
    public float xRange = 2f;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnLogic();
            spawnTimer = 0f;
        }

        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 20f)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void IncreaseDifficulty()
    {
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= difficultyStep;

            if (spawnRate < minSpawnRate) spawnRate = minSpawnRate;

            Debug.Log("Attention, ça tombe plus vite ! Nouveau spawnRate : " + spawnRate);
        }
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
