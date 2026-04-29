using UnityEngine;

public class TrashInteraction : MonoBehaviour
{
    private Vector3 startPos;
    private bool isTargeted = false;
    public float minDragDistance = 0.5f;

    void OnMouseDown()
    {
        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if (isTargeted) return;

        Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float verticalMovement = endPos.y - startPos.y;

        if (verticalMovement > minDragDistance)
        {
            isTargeted = true;
            HookSpawner spawner = GameObject.FindObjectOfType<HookSpawner>();
            if (spawner != null)
            {
                spawner.SpawnHook(this.gameObject);
            }
        }
    }
}