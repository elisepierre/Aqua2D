using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadStory()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void LoadChoice()
    {
        SceneManager.LoadScene("ChoiceScene");
    }
}