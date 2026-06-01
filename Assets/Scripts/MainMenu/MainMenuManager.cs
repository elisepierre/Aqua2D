using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f; // Sécurité absolue pour réveiller les boutons

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }
    }

    public void LoadStory()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void LoadChoice()
    {
        SceneManager.LoadScene("LinkScene");
    }

    public void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
        }
    }
}