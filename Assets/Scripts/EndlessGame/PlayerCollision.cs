using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject loosePanel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("LOSER !");
            loosePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (other.CompareTag("Shell"))
        {
            score++;
            scoreText.text = score.ToString();
            Destroy(other.gameObject);
        }
    }
}