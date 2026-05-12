using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject playButton;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        playButton.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }
}