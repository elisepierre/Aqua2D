using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Elements")]
    public TextMeshProUGUI timeText;

    public GameObject pauseButtonObject;
    public GameObject playButtonObject;

    public Image[] heartImages;

    private float timer = 0f;
    private int lives = 3;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateLivesUI();

        pauseButtonObject.SetActive(true);
        playButtonObject.SetActive(false);
    }

    void Update()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime;
            UpdateTimeUI();
        }
    }

    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();
        if (lives <= 0) GameOver();
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < lives)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;

            pauseButtonObject.SetActive(false);
            playButtonObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;

            pauseButtonObject.SetActive(true);
            playButtonObject.SetActive(false);
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}