using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject loosePanel;
    public GameObject collectAnimPrefab;
    public RectTransform scoreIcon;

    void Awake()
    {
        score = 0;
    }
    void Start()
    {
        Time.timeScale = 1f;
        if (scoreText != null)
        {
            scoreText.text = "0";
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("LOSER !");

            if (DataManager.Instance != null)
            {
                DataManager.Instance.AddShellsToTotal(score);
            }

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
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LinkScene");
    }
}