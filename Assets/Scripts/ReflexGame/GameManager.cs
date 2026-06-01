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
    public TextMeshProUGUI gameOverTitleText; // TXT_GameOverTitle
    public TextMeshProUGUI endTimerText;      // TXT_FinalTimer (affichera "Score: Xs")
    public TextMeshProUGUI endShellsText;     // TXT_FinalShells
    public TextMeshProUGUI bestTimerText;     // TXT_BestTimer (affichera "Best: Xs")

    [Header("HUD Elements to Hide on GameOver")]
    public GameObject[] hudElementsToHide; // Glisse ici le bouton Pause, le Timer du jeu, le Score, l'icone Shell, etc.

    [Header("Collect Settings")]
    public RectTransform globalScoreIcon;
    public GameObject collectAnimPrefab;

    private float timer = 0f;
    private int score = 0;
    private bool isPaused = false;
    private bool isGameOver = false;

    void Awake()
    {
        // On ne survit pas aux scènes. L'instance est celle de la scène actuelle.
        instance = this;
    }

    void Start()
    {
        // C'EST CETTE LIGNE QUI DÉBLOQUE TOUT
        Time.timeScale = 1f;

        isGameOver = false; // On remet les flags à zéro
        isPaused = false;
        timer = 0f;
        score = 0;

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

        // 1. ARRÊT DE LA MUSIQUE ET SON DE GAME OVER
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
        }

        // 2. SAUVEGARDE DES COQUILLAGES GLOBALEMENT
        if (DataManager.Instance != null)
        {
            DataManager.Instance.AddShellsToTotal(score);
        }

        // 3. GESTION DU RECORD DE SURVIE (BEST TIME)
        int currentTime = Mathf.FloorToInt(timer);
        // Utilise une clé unique pour ce mini-jeu, ex: "BestTime_Catch"
        int bestTime = PlayerPrefs.GetInt("BestTime_Catch", 0);

        if (currentTime > bestTime)
        {
            PlayerPrefs.SetInt("BestTime_Catch", currentTime);
            bestTime = currentTime;
        }

        // 4. MASQUER LES ÉLÉMENTS DU HUD
        foreach (GameObject obj in hudElementsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }

        // 5. AFFICHAGE DES TEXTES TRADUITS
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);

        // Titre : "Fin de partie"
        if (gameOverTitleText != null)
        {
            gameOverTitleText.text = (lang == 1) ? "遊戲結束" : (lang == 2 ? "FIN DE PARTIE" : "GAME OVER");
        }

        // Score final : "Score: Xs"
        if (endTimerText != null)
        {
            string scoreLabel = (lang == 1) ? "分數" : (lang == 2 ? "Score" : "Score");
            endTimerText.text = $"{scoreLabel}: {currentTime}s";
        }

        // Coquillages récoltés durant la partie
        if (endShellsText != null)
        {
            endShellsText.text = score.ToString();
        }

        // Meilleur temps : "Best: Xs"
        if (bestTimerText != null)
        {
            string bestLabel = (lang == 1) ? "最高紀錄" : "Best";
            bestTimerText.text = $"{bestLabel}: {bestTime}s";
        }

        // 6. PAUSE ET ACTIVATION DU PANEL
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