using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionSlot : MonoBehaviour
{
    public CollectableItem itemData; // La fiche de l'objet
    public Image iconImage;          // L'image du slot
    public TextMeshProUGUI nameText; // Le texte du nom

    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        // On vérifie si l'ID est sauvegardé comme "1" dans la mémoire
        bool isUnlocked = PlayerPrefs.GetInt("Unlocked_" + itemData.itemID, 0) == 1;

        if (isUnlocked)
        {
            iconImage.color = Color.white; // Couleur normale

            // --- LA CORRECTION EST ICI ---
            if (LanguageManager.instance != null)
            {
                // On demande au LanguageManager le nom traduit selon l'ID
                nameText.text = LanguageManager.instance.GetTranslatedItemName(itemData.itemID);
            }
            else
            {
                // Si le manager est absent (test scène), on garde le nom par défaut
                nameText.text = itemData.itemName;
            }
        }
        else
        {
            iconImage.color = Color.black; // Silhouette noire

            // Traduction du "???" selon la langue
            int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
            nameText.text = (lang == 1) ? "？？？" : "???";
        }

        // On s'assure que la police change aussi pour le chinois
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