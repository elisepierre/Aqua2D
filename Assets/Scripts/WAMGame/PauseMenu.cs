using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseButton;
    public GameObject pausePanel;

    public void PauseGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void GoToMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("LinkScene");
    }
}