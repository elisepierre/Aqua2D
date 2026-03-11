using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;

    private float timer = 0f;
    private int lives = 3;
    private bool isPaused = false;

    void Awake() { instance = this; }

    void Update()
    {
        timer += Time.deltaTime;
        timeText.text = Mathf.FloorToInt(timer).ToString();
    }

    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();
        if (lives <= 0) GameOver();
    }

    void UpdateLivesUI()
    {
        string hearts = "";
        for (int i = 0; i < lives; i++) hearts += "O";
        livesText.text = hearts;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}