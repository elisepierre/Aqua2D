using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject[] collectibles;
    public float[] lanes = new float[] { -2f, 0f, 2f };
    public float spawnRate = 2f;

    void Start()
    {
        InvokeRepeating("SpawnSomething", 1f, spawnRate);
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