using UnityEngine;

public class DangerZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash"))
        {
            GameManager.instance.TriggerGameOver();
        }
        else if (other.CompareTag("Shell"))
        {
            Destroy(other.gameObject);
        }
    }
}