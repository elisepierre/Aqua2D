using UnityEngine;
using TMPro;

public class WhackManager : MonoBehaviour
{
    public static WhackManager Instance;

    public GameObject[] badGuys;
    public GameObject[] shells;
    public Transform[] holePositions;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public RectTransform globalScoreIcon;


    private int score = 0;
    private bool[] holeOccupied;

    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;

    void Update()
    {
        if (Time.timeScale > 0)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Awake()
    {
        Instance = this;
        if (holePositions != null)
        {
            holeOccupied = new bool[holePositions.Length];
        }
    }

    void Start()
    {
        InvokeRepeating("SpawnLogic", 1f, 2f);
    }

    void SpawnLogic()
    {
        System.Collections.Generic.List<int> availableHoles = new System.Collections.Generic.List<int>();

        for (int i = 0; i < holePositions.Length; i++)
        {
            if (!holeOccupied[i])
            {
                availableHoles.Add(i);
            }
        }

        if (availableHoles.Count == 0) return;

        int holeIndex = availableHoles[Random.Range(0, availableHoles.Count)];

        holeOccupied[holeIndex] = true;

        GameObject prefab;

        if (Random.value < 0.7f)
            prefab = badGuys[Random.Range(0, badGuys.Length)];
        else
            prefab = shells[Random.Range(0, shells.Length)];

        GameObject spawned = Instantiate(prefab, holePositions[holeIndex].position, Quaternion.identity);

        spawned.GetComponent<WhackableObject>().currentHoleIndex = holeIndex;

        SpriteRenderer sr = spawned.GetComponent<SpriteRenderer>();

        if (holeIndex >= 3)
            sr.sortingOrder = 2;
        else
            sr.sortingOrder = 0;
    }

    public void ReleaseHole(int index)
    {
        if (index >= 0 && index < holeOccupied.Length)
        {
            holeOccupied[index] = false;
        }
    }

    public void AddScore(int pts)
    {
        score += pts;
        scoreText.text = "" + score;
    }

    public void GameOver()
    {
        CancelInvoke();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
}