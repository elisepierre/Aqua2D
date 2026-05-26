using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public GameObject shellPrefab;

    [Header("Réglages de Vitesse")]
    public float spawnRate = 1.5f;      // Temps entre 2 objets au début
    public float minSpawnRate = 0.4f;   // Vitesse maximum (ne pas descendre trop bas)
    public float difficultyStep = 0.15f; // On réduit le temps de 0.15s toutes les 20s
    public float xRange = 2f;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        // 1. Gestion de l'apparition (Spawn)
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnLogic();
            spawnTimer = 0f;
        }

        // 2. Gestion de la difficulté toutes les 20s
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

            // Sécurité pour ne pas être impossible
            if (spawnRate < minSpawnRate) spawnRate = minSpawnRate;

            Debug.Log("Attention, ça tombe plus vite ! Nouveau spawnRate : " + spawnRate);
        }
    }

    void SpawnLogic()
    {
        GameObject prefabToSpawn;

        // 30% de chance d'avoir un coquillage, 70% de déchets
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