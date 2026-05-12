using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public float spawnRate = 1.5f;
    public float xRange = 4f;

    void Start()
    {
        InvokeRepeating("SpawnTrash", 0f, spawnRate);
    }

    void SpawnTrash()
    {
        int randomIndex = Random.Range(0, trashPrefabs.Length);

        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);

        Instantiate(trashPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - xRange, transform.position.y, 0),
                        new Vector3(transform.position.x + xRange, transform.position.y, 0));
    }
}