using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] collectibles;
    public float[] lanes = new float[] { -2f, 0f, 2f };

    [Header("Réglages de Difficulté")]
    public float spawnRate = 2f;
    public float minSpawnRate = 1f;
    public float difficultyStep = 0.05f;

    private float spawnTimer = 0f;
    private float difficultyTimer = 0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnSomething();
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

            Debug.Log("La difficulté augmente ! Nouveau spawnRate : " + spawnRate);
        }
    }

    void SpawnSomething()
    {
        int laneIndex = Random.Range(0, lanes.Length);
        GameObject prefab;

        if (Random.value > 0.3f)
        {
            prefab = obstacles[Random.Range(0, obstacles.Length)];

            if (prefab.name.Contains("big"))
            {
                if (laneIndex > 1) laneIndex = 1;
                float posX = (lanes[laneIndex] + lanes[laneIndex + 1]) / 2f;
                Instantiate(prefab, new Vector3(posX, 6f, 0f), Quaternion.identity);
            }
            else
            {
                Instantiate(prefab, new Vector3(lanes[laneIndex], 6f, 0f), Quaternion.identity);
            }
        }
        else
        {
            prefab = collectibles[Random.Range(0, collectibles.Length)];
            Instantiate(prefab, new Vector3(lanes[laneIndex], 6f, 0f), Quaternion.identity);
        }
    }
}
