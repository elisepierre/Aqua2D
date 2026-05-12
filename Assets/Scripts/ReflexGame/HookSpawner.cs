using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    public GameObject hookPrefab;

    public void SpawnHook(GameObject targetTrash)
    {

        float targetX = targetTrash.transform.position.x;
        Vector3 spawnPos = new Vector3(targetX, 4.5f, 0);

        GameObject newHook = Instantiate(hookPrefab, spawnPos, Quaternion.identity);
        newHook.GetComponent<HookChaser>().GrabTrashAtPosition(targetTrash);
    }
}