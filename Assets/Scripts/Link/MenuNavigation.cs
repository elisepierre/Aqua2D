using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void LoadChoiceScene()
    {
        SceneManager.LoadScene("ChoiceScene");
    }

    public void LoadGachaponScene()
    {
        SceneManager.LoadScene("GachaponScene");
    }

    public void LoadAquariumScene()
    {
        SceneManager.LoadScene("AquariumScene");
    }

    public void LoadLinkScene()
    {
        SceneManager.LoadScene("LinkScene");
    }

}