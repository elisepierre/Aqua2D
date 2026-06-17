using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionSlot : MonoBehaviour
{
    public CollectableItem itemData;
    public Image iconImage;
    public TextMeshProUGUI nameText;

    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        bool isUnlocked = PlayerPrefs.GetInt("Unlocked_" + itemData.itemID, 0) == 1;

        if (isUnlocked)
        {
            iconImage.color = Color.white;

            if (LanguageManager.instance != null)
            {
                nameText.text = LanguageManager.instance.GetTranslatedItemName(itemData.itemID);
            }
            else
            {
                nameText.text = itemData.itemName;
            }
        }
        else
        {
            iconImage.color = Color.black;

            int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            nameText.text = (lang == 1) ? "？？？" : "???";
        }

        UpdateFont();
    }

    private void UpdateFont()
    {
        if (LanguageManager.instance != null && nameText != null)
        {
            int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            nameText.font = (lang == 1) ? LanguageManager.instance.chineseFontAsset : LanguageManager.instance.englishFontAsset;
            nameText.UpdateFontAsset();
        }
    }
}
