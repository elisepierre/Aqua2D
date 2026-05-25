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
            nameText.text = itemData.itemName;
        }
        else
        {
            iconImage.color = Color.black; // Silhouette noire
            nameText.text = "???";
        }
    }
}