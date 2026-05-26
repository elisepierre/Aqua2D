using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    void OnEnable()
    {
        CollectionSlot[] allSlots = FindObjectsOfType<CollectionSlot>();
        foreach (CollectionSlot slot in allSlots)
        {
            slot.UpdateDisplay();
        }
    }
}