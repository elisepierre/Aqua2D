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

    public void LoadCollectionScene()
    {
        SceneManager.LoadScene("CollectionScene");
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLinkScene()
    {
        GachaponManager gacha = FindObjectOfType<GachaponManager>();

        if (gacha != null)
        {
            if (gacha.isUiActive)
            {
                Debug.Log("Retour bloqué : Tirage en cours !");
                return;
            }
        }

        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LinkScene");
    }

}