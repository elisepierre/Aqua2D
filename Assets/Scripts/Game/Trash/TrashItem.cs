using UnityEngine;

public class TrashItem : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        Destroy(gameObject);
    }

    void Update()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}