using UnityEngine;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    public TextMeshProUGUI playText;
    public TextMeshProUGUI settingsText;

    void Start()
    {
        SetLanguage(PlayerPrefs.GetInt("Language", 0));
    }

    public void SetLanguage(int langIndex)
    {
        PlayerPrefs.SetInt("Language", langIndex);

        if (langIndex == 0)
        {
            playText.text = "PLAY";
            settingsText.text = "SETTINGS";
        }
        else
        {
            playText.text = "開始遊戲";
            settingsText.text = "設定";
        }
    }
}