using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [Header("Font Assets")]
    public TMP_FontAsset englishFontAsset;
    public TMP_FontAsset chineseFontAsset;

    [Header("Text Elements to Localize")]
    public List<TextMeshProUGUI> allTextElements = new List<TextMeshProUGUI>();

    [Header("Specific Text Objects (for content)")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI playText;
    public TextMeshProUGUI settingsText;
    public TextMeshProUGUI englishText;
    public TextMeshProUGUI chineseText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        int savedLang = PlayerPrefs.GetInt("SelectedLanguage", 0);
        ApplyLanguage(savedLang);
    }

    public void SetEnglish()
    {
        ApplyLanguage(0);
    }

    public void SetChinese()
    {
        ApplyLanguage(1);
    }

    private void ApplyLanguage(int index)
    {
        PlayerPrefs.SetInt("SelectedLanguage", index);

        TMP_FontAsset fontToApply = null;

        if (index == 0)
        {
            fontToApply = englishFontAsset;

            if (playText != null) playText.text = "PLAY";
            if (settingsText != null) settingsText.text = "SETTINGS";
            if (englishText != null) englishText.text = "ENGLISH";
            if (chineseText != null) chineseText.text = "CHINESE";
        }
        else if (index == 1)
        {
            fontToApply = chineseFontAsset;

            if (playText != null) playText.text = "開始遊戲";
            if (settingsText != null) settingsText.text = "設定";
            if (englishText != null) englishText.text = "英文";
            if (chineseText != null) chineseText.text = "繁體中文";
        }

        if (fontToApply != null)
        {
            foreach (TextMeshProUGUI textElement in allTextElements)
            {
                if (textElement != null)
                {
                    textElement.font = fontToApply;
                    textElement.UpdateFontAsset();
                }
            }
        }
    }
}