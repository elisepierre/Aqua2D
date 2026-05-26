using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WhackManager : MonoBehaviour
{
    public static WhackManager Instance;

    public GameObject[] badGuys;
    public GameObject[] shells;
    public Transform[] holePositions;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public RectTransform globalScoreIcon;
    public GameObject sirenPrefab;

    [Header("Difficulté Progressive")]
    public float globalActiveTime = 3f;
    public float minActiveTime = 0.8f;
    public float speedStep = 0.3f;

    [Header("Fréquence d'apparition")]
    public float spawnRate = 2f;         // Délai entre les spawns au début
    public float minSpawnRate = 0.5f;    // Délai minimum
    public float spawnStep = 0.2f;       // Réduction du délai toutes les 20s

    private float difficultyTimer = 0f;
    private float spawnTimer = 0f;
    private int score = 0;
    private bool[] holeOccupied;

    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;

    void Awake()
    {
        Instance = this;
        if (holePositions != null) holeOccupied = new bool[holePositions.Length];
    }

    void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        // On ne fait plus InvokeRepeating ici !
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();

        // 1. Gestion du Spawn (Fréquence)
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnLogic();
            spawnTimer = 0f;
        }

        // 2. Gestion de la Difficulté toutes les 20s
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 20f)
        {
            IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }

    void IncreaseDifficulty()
    {
        // Réduit le temps où ils restent dehors
        if (globalActiveTime > minActiveTime)
        {
            globalActiveTime -= speedStep;
            if (globalActiveTime < minActiveTime) globalActiveTime = minActiveTime;
        }

        // Réduit le temps entre chaque apparition (spawn plus vite)
        if (spawnRate > minSpawnRate)
        {
            spawnRate -= spawnStep;
            if (spawnRate < minSpawnRate) spawnRate = minSpawnRate;
        }

        Debug.Log($"Difficulté UP ! Spawn: {spawnRate}s | Durée: {globalActiveTime}s");
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        if (timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SpawnLogic()
    {
        List<int> availableHoles = new List<int>();
        for (int i = 0; i < holePositions.Length; i++)
        {
            if (!holeOccupied[i]) availableHoles.Add(i);
        }

        if (availableHoles.Count == 0) return;

        int holeIndex = availableHoles[Random.Range(0, availableHoles.Count)];
        holeOccupied[holeIndex] = true;

        GameObject prefab;
        float chance = Random.value;

        if (chance < 0.1f) // 10% de chance que ce soit la sirène
        {
            prefab = sirenPrefab;
        }
        else if (chance < 0.7f) // Environ 60% de méchants
        {
            prefab = badGuys[Random.Range(0, badGuys.Length)];
        }
        else // Le reste en coquillages
        {
            prefab = shells[Random.Range(0, shells.Length)];
        }

        GameObject spawned = Instantiate(prefab, holePositions[holeIndex].position, Quaternion.identity);

        // On vérifie si c'est la sirène ou un objet classique pour donner l'index
        var whackable = spawned.GetComponent<WhackableObject>();
        if (whackable != null) whackable.currentHoleIndex = holeIndex;

        var siren = spawned.GetComponent<SirenObject>();
        if (siren != null) siren.currentHoleIndex = holeIndex;

        // Gestion du tri des sprites
        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = (holeIndex >= 3) ? 2 : 0;
    }
    public void ReleaseHole(int index)
    {
        if (index >= 0 && index < holeOccupied.Length) holeOccupied[index] = false;
    }

    public void AddScore(int pts)
    {
        score += pts;
        if (scoreText != null) scoreText.text = "" + score;
    }

    public void GameOver()
    {
        if (Time.timeScale == 0) return;
        if (DataManager.Instance != null) DataManager.Instance.AddShellsToTotal(score);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
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