using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    private System.Collections.Generic.HashSet<int> collectedShells = new System.Collections.Generic.HashSet<int>();
    public TextMeshProUGUI scoreText;
    public GameObject loosePanel;
    public GameObject collectAnimPrefab;
    public RectTransform scoreIcon;
    private int lastFrameChecked = -1;
    private float lastCollectTime = 0f;
    private float collectDelay = 0.1f;

    void Awake()
    {
        score = 0;
    }
    void Start()
    {
        Time.timeScale = 1f;

        if (scoreText != null) scoreText.text = "0";
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.rockHitClip);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
                AudioManager.Instance.StopMusic();
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.TriggerGameOver();
            }
            Time.timeScale = 0f;
            return;
        }
        else if (other.CompareTag("Shell"))
        {
            if (Time.time < lastCollectTime + collectDelay) return;

            lastCollectTime = Time.time;
            int shellID = other.gameObject.GetInstanceID();

            if (collectedShells.Contains(shellID)) return;

            collectedShells.Add(shellID);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.shellClip);
            }

            other.tag = "Untagged";
            Collider2D shellCollider = other.GetComponent<Collider2D>();
            if (shellCollider != null) shellCollider.enabled = false;

            GameObject anim = Instantiate(collectAnimPrefab, other.transform.position, Quaternion.identity);
            if (anim != null)
            {
                anim.GetComponent<EndlessCollectAnimation>().StartAnimation(scoreIcon);
            }

            score++;
            if (scoreText != null) scoreText.text = score.ToString();

            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(1);
            }

            Destroy(other.gameObject);
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.homeClip);
            AudioManager.Instance.StopMusic();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("LinkScene");
    }
}
