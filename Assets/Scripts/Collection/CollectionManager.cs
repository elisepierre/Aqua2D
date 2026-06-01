using UnityEngine;

public class CollectionManager : MonoBehaviour
{

    void OnEnable()
    {
        // 1. On met à jour les slots (images, cadenas, etc.)
        CollectionSlot[] allSlots = FindObjectsOfType<CollectionSlot>();
        foreach (CollectionSlot slot in allSlots)
        {
            slot.UpdateDisplay();
        }

        // 2. IMPORTANT : On demande au LanguageManager de traduire les textes
        // Maintenant que les slots sont potentiellement activés, 
        // le LanguageManager peut les trouver et changer leur texte.
        if (LanguageManager.instance != null)
        {
            LanguageManager.instance.RefreshSceneText();
        }
    }
}