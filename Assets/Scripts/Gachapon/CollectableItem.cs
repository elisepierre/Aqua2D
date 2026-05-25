using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Gacha/Item")]
public class CollectableItem : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemSprite;
    public bool isFish;
}