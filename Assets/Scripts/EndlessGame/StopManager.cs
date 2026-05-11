using UnityEngine;
using UnityEngine.UI;

public class StopManager : MonoBehaviour
{
    public GameObject stopButton;
    public GameObject playButton;
    public GameObject loosePanel;

    public void StopGame()
    {
        if (loosePanel != null && loosePanel.activeSelf) return;

        Time.timeScale = 0f;
        stopButton.SetActive(false);
        playButton.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        stopButton.SetActive(true);
        playButton.SetActive(false);
    }
}
