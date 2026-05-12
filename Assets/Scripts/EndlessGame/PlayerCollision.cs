using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject loosePanel;
    public GameObject collectAnimPrefab;
    public RectTransform scoreIcon;

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
            Collider2D shellCollider = other.GetComponent<Collider2D>();
            if (shellCollider != null && shellCollider.enabled)
            {
                shellCollider.enabled = false;

                GameObject anim = Instantiate(collectAnimPrefab, other.transform.position, Quaternion.identity);
                anim.GetComponent<EndlessCollectAnimation>().StartAnimation(scoreIcon);

                score++;
                scoreText.text = score.ToString();
                Destroy(other.gameObject);
            }
        }
    }
}