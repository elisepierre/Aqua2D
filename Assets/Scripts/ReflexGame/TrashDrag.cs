using UnityEngine;

public class TrashDrag : MonoBehaviour
{
    private bool isTargeted = false;

    void OnMouseUp()
    {
        if (isTargeted)
        {
            Debug.Log("Ce déchet est déjà en cours de pêche !");
            return;
        }
        isTargeted = true;

        HookSpawner spawner = GameObject.FindObjectOfType<HookSpawner>();
        if (spawner != null)
        {
            spawner.SpawnHook(this.gameObject);
        }
    }
}