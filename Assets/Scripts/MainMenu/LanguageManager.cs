using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [Header("Font Assets")]
    public TMP_FontAsset englishFontAsset;
    public TMP_FontAsset chineseFontAsset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        RefreshSceneText();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshSceneText();
    }

    public void SetLanguage(int index)
    {
        PlayerPrefs.SetInt("SelectedLanguage", index);
        RefreshSceneText();
    }

    public void RefreshSceneText()
    {
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);

        TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (var txt in allTexts)
        {
            if (txt == null) continue;

            if (lang == 1 && txt.gameObject.name.StartsWith("TXT_"))
            {
                txt.font = chineseFontAsset;
            }
            else
            {
                txt.font = englishFontAsset;
            }

            txt.UpdateFontAsset();
        }

        TranslateByName("TXT_Play", lang == 0 ? "PLAY" : "開始遊戲");
        TranslateByName("TXT_Continue", lang == 0 ? "CONTINUE" : "繼續");
        TranslateByName("TXT_Settings", lang == 0 ? "SETTINGS" : "設定");
        TranslateByName("TXT_English", lang == 0 ? "ENGLISH" : "英文");
        TranslateByName("TXT_Chinese", lang == 0 ? "CHINESE" : "繁體中文");

        TranslateByName("TXT_ChoiceTitle", lang == 0 ? "CHOOSE A MINI-GAME" : "選擇小遊戲");
        TranslateByName("TXT_Game1", lang == 0 ? "CLEAN OCEAN" : "清潔海洋");
        TranslateByName("TXT_Game2", lang == 0 ? "SWIM RUSH" : "極速游泳");
        TranslateByName("TXT_Game3", lang == 0 ? "FISHER WHACK" : "敲打漁夫");
    }

    private void TranslateByName(string objName, string translation)
    {
        GameObject go = GameObject.Find(objName);
        if (go != null && go.GetComponent<TextMeshProUGUI>() != null)
        {
            go.GetComponent<TextMeshProUGUI>().text = translation;
        }
    }

    public string GetStoryDialog(int step, int lang)
    {
        if (lang == 0)
        {
            switch (step)
            {
                case 0: return "Our home is beautiful and peaceful...";
                case 1: return "But suddenly... trash is invading our water!";
                case 2: return "Fishermen are taking my friends away...";
                case 3: return "I am all alone now...";
                case 4: return "Please, traveler, you must help us clean and save the ocean!";
                default: return "";
            }
        }
        else
        {
            switch (step)
            {
                case 0: return "我們的家園原本既美麗又和平...";
                case 1: return "但突然間... 垃圾正在入侵我們的水域！";
                case 2: return "漁夫正在抓走我的朋友們...";
                case 3: return "現在只剩下我一個人了...";
                case 4: return "旅人，拜託你，請救救我們，清理並守護這片海洋！";
                default: return "";
            }
        }
    }
}