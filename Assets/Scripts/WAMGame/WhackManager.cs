using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WhackManager : MonoBehaviour
{
    public static WhackManager Instance;

    [Header("Prefabs & Positions")]
    public GameObject[] badGuys;
    public GameObject[] shells;
    public Transform[] holePositions;
    public GameObject sirenPrefab;

    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public RectTransform globalScoreIcon;

    [Header("Difficulté Progressive")]
    public float globalActiveTime = 3f;
    public float minActiveTime = 0.8f;
    public float speedStep = 0.3f;

    [Header("Fréquence d'apparition")]
    public float spawnRate = 2f;
    public float minSpawnRate = 0.5f;
    public float spawnStep = 0.2f;

    private float difficultyTimer = 0f;
    private float spawnTimer = 0f;
    private int score = 0;
    private bool[] holeOccupied;
    private float elapsedTime = 0f;

    [Header("GameOver UI")]
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI endTimerText;
    public TextMeshProUGUI endShellsText;
    public TextMeshProUGUI bestTimerText;

    [Header("HUD Elements to Hide")]
    public GameObject[] hudElementsToHide;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (holePositions != null)
        {
            holeOccupied = new bool[holePositions.Length];
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayWhackMusic();
        }

        Time.timeScale = 1f;
        score = 0;
        elapsedTime = 0f;
        spawnTimer = 0f;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnLogic();
            spawnTimer = 0f;
        }

        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 20f)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void IncreaseDifficulty()
    {
        if (globalActiveTime > minActiveTime)
        {
            globalActiveTime -= speedStep;
            if (globalActiveTime < minActiveTime) globalActiveTime = minActiveTime;
        }

        if (spawnRate > minSpawnRate)
        {
            spawnRate -= spawnStep;
            if (spawnRate < minSpawnRate) spawnRate = minSpawnRate;
        }
    }

    void UpdateTimerDisplay()
    {
        int totalSeconds = Mathf.FloorToInt(elapsedTime);
        if (timerText != null) timerText.text = totalSeconds + "s";
    }

    void SpawnLogic()
    {
        if (holePositions == null || holeOccupied == null || holePositions.Length == 0) return;

        List<int> availableHoles = new List<int>();
        for (int i = 0; i < holePositions.Length; i++)
        {
            if (!holeOccupied[i]) availableHoles.Add(i);
        }

        if (availableHoles.Count == 0) return;

        int holeIndex = availableHoles[Random.Range(0, availableHoles.Count)];
        holeOccupied[holeIndex] = true;

        GameObject prefab = ChoosePrefab();
        if (prefab == null) return;

        GameObject spawned = Instantiate(prefab, holePositions[holeIndex].position, Quaternion.identity);

        WhackableObject whackable = spawned.GetComponent<WhackableObject>();
        if (whackable != null) whackable.currentHoleIndex = holeIndex;

        SirenObject siren = spawned.GetComponent<SirenObject>();
        if (siren != null) siren.currentHoleIndex = holeIndex;

        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        if (sr != null)
        {

            if (holeIndex < 3)
            {
                sr.sortingOrder = 2;
            }
            else
            {
                sr.sortingOrder = 5;
            }
        }
    }

    GameObject ChoosePrefab()
    {
        float chance = Random.value;
        if (chance < 0.5f) return badGuys[Random.Range(0, badGuys.Length)];
        if (chance < 0.7f) return sirenPrefab;
        return shells[Random.Range(0, shells.Length)];
    }

    public void ReleaseHole(int index)
    {
        if (index >= 0 && index < holeOccupied.Length)
            holeOccupied[index] = false;
    }

    public void AddScore(int pts)
    {
        score += pts;
        if (scoreText != null) scoreText.text = "" + score;
    }

    public void GameOver()
    {
        if (Time.timeScale == 0) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
        }

        if (DataManager.Instance != null) DataManager.Instance.AddShellsToTotal(score);

        int currentTime = Mathf.FloorToInt(elapsedTime);
        int bestTime = PlayerPrefs.GetInt("BestTime_Whack", 0);

        if (currentTime > bestTime)
        {
            PlayerPrefs.SetInt("BestTime_Whack", currentTime);
            bestTime = currentTime;
        }

        foreach (GameObject obj in hudElementsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }

        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);

        gameOverTitleText.text = (lang == 1) ? "遊戲結束" : (lang == 2 ? "FIN DE PARTIE" : "GAME OVER");

        string scoreLabel = (lang == 1) ? "分數" : (lang == 2 ? "Score" : "Score");
        endTimerText.text = $"{scoreLabel}: {currentTime}s";

        endShellsText.text = score.ToString();

        string bestLabel = (lang == 1) ? "最高紀錄" : "Best";
        bestTimerText.text = $"{bestLabel}: {bestTime}s";

        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
