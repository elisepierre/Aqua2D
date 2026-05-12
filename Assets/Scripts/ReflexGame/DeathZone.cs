using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trash"))
        {
            GameManager.instance.LoseLife();
            Destroy(other.gameObject);
        }
    }
}