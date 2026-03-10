using UnityEngine;

public class TrashItem : MonoBehaviour
{
    void OnMouseDown()
    {
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