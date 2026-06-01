using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StopManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject stopButton;
    public GameObject pausePanel;
    public GameObject loosePanel;

    public void StopGame()
    {
        if (loosePanel != null && loosePanel.activeSelf) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
        }

        Time.timeScale = 0f;
        stopButton.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        stopButton.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;       
        SceneManager.LoadScene("LinkScene");
    }
}