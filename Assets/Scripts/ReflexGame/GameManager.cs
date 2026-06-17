using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    [Header("GameOver UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI endTimerText;
    public TextMeshProUGUI endShellsText;
    public TextMeshProUGUI bestTimerText;

    [Header("HUD Elements to Hide on GameOver")]
    public GameObject[] hudElementsToHide;

    [Header("Collect Settings")]
    public RectTransform globalScoreIcon;
    public GameObject collectAnimPrefab;

    public enum GameType { Reflex, Endless }
    [Header("Configuration du son")]
    public GameType modeDeJeu;

    private float timer = 0f;
    private int score = 0;
    private bool isPaused = false;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isPaused = false;
        timer = 0f;
        score = 0;

        if (AudioManager.Instance != null)
        {
            if (modeDeJeu == GameType.Endless)
            {
                Debug.Log("Lancement Musique : ENDLESS");
                AudioManager.Instance.PlayEndlessMusic();
            }
            else
            {
                Debug.Log("Lancement Musique : REFLEX");
                AudioManager.Instance.PlayCatchMusic();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
        }

        if (DataManager.Instance != null)
        {
            DataManager.Instance.AddShellsToTotal(score);
        }

        int currentTime = Mathf.FloorToInt(timer);
        int bestTime = PlayerPrefs.GetInt("BestTime_Catch", 0);

        if (currentTime > bestTime)
        {
            PlayerPrefs.SetInt("BestTime_Catch", currentTime);
            bestTime = currentTime;
        }

        foreach (GameObject obj in hudElementsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }

        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);

        if (gameOverTitleText != null)
        {
            gameOverTitleText.text = (lang == 1) ? "遊戲結束" : (lang == 2 ? "FIN DE PARTIE" : "GAME OVER");
        }

        if (endTimerText != null)
        {
            string scoreLabel = (lang == 1) ? "分數" : (lang == 2 ? "Score" : "Score");
            endTimerText.text = $"{scoreLabel}: {currentTime}s";
        }

        if (endShellsText != null)
        {
            endShellsText.text = score.ToString();
        }

        if (bestTimerText != null)
        {
            string bestLabel = (lang == 1) ? "最高紀錄" : "Best";
            bestTimerText.text = $"{bestLabel}: {bestTime}s";
        }

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
        }
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
