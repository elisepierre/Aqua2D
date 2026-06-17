using UnityEngine;
using System.Collections.Generic;

public class AquariumManager : MonoBehaviour
{
    public List<CollectableItem> allPossibleItems;

    [Header("Points de placement précis")]
    public List<Transform> spawnPoints;

    public GameObject fishPrefab;
    public GameObject staticPrefab;

    void Start()
    {
        SpawnUnlockedItems();
    }

    void SpawnUnlockedItems()
    {
        foreach (CollectableItem item in allPossibleItems)
        {
            if (PlayerPrefs.GetInt("Unlocked_" + item.itemID, 0) == 1)
            {
                if (item.isFish)
                {
                    SpawnFish(item);
                }
                else
                {
                    SpawnStaticAtCorrectPoint(item);
                }
            }
        }
    }

    void SpawnStaticAtCorrectPoint(CollectableItem item)
    {
        Transform targetPoint = spawnPoints.Find(p => p.name.Contains(item.itemID));

        if (targetPoint != null)
        {
            GameObject newStatic = Instantiate(staticPrefab, targetPoint.position, Quaternion.identity);
            
            if (targetPoint.GetComponent<RectTransform>() != null)
            {
                newStatic.transform.SetParent(targetPoint, false);
                newStatic.transform.localPosition = Vector3.zero;
            }

            SpriteRenderer sr = newStatic.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = item.itemSprite;
                sr.sortingOrder = 10;
            }

            newStatic.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }
        else
        {
            Debug.LogWarning("Attention : Aucun point de spawn trouvé pour " + item.itemID);
        }
    }

    void SpawnFish(CollectableItem item)
    {
        GameObject newFish = Instantiate(fishPrefab, Vector3.zero, Quaternion.identity);
        newFish.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        newFish.GetComponent<SpriteRenderer>().sortingOrder = 20;
        newFish.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        newFish.AddComponent<FishAI>();
    }
}
