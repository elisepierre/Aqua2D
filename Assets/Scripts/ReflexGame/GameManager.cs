using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    [Header("Collect Settings")]
    public RectTransform globalScoreIcon;
    public GameObject collectAnimPrefab;

    private float timer = 0f;
    private int score = 0;
    private bool isPaused = false;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si on revient dans la scène, on détruit le nouveau pour garder l'ancien
            Destroy(gameObject);
            return; // TRÈS IMPORTANT : on s'arrête là
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        // 1. ON LANCE LA MUSIQUE DU DEEP SEA CATCH
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCatchMusic();
        }
    }

    void Update()
    {
        if (!isPaused && !isGameOver)
        {
            timer += Time.deltaTime;
            UpdateTimeUI();
        }
    }

    void UpdateTimeUI()
    {
        int totalSeconds = Mathf.FloorToInt(timer);
        timeText.text = totalSeconds + "s";
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "" + score;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 2. ARRÊT DE LA MUSIQUE ET SON DE GAME OVER
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
        }

        if (DataManager.Instance != null)
        {
            DataManager.Instance.AddShellsToTotal(score);
        }

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    // À lier sur ton bouton Pause dans l'UI
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // 3. SON DE PAUSE
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
        }
    }

    // À lier sur ton bouton Home (Retour au menu principal)
    public void GoToMenu()
    {
        // 4. SON DU BOUTON HOME + SÉCURITÉ ARRÊT MUSIQUE
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.homeClip);
            AudioManager.Instance.StopMusic();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("LinkScene");
    }

    public void RestartGame()
    {
        // 5. RELANCER LA MUSIQUE AU RESTART
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCatchMusic();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}