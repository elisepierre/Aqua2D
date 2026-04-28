using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadStory()
    {
        SceneManager.LoadScene("StoryScene");
    }
}