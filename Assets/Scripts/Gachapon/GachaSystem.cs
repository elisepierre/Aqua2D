using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GachaponManager : MonoBehaviour
{
    [Header("UI & Data")]
    public TextMeshProUGUI resultNameText;
    public UnityEngine.UI.Image resultImage;
    public GameObject prizePanel;
    public GameObject spinText;

    [Header("Listes de Collection")]
    public List<CollectableItem> allFishes;
    public List<CollectableItem> allDecors;

    [Header("Animation")]
    public GameObject[] ballPrefabs;
    public Transform spawnPoint;
    public Transform centerScreenPos;

    private GameObject currentBall;
    [HideInInspector] public bool isUiActive = false;

    [Header("New Prize UI")]
    public GameObject newTextObject;

    void Start()
    {
        if (DataManager.Instance != null) DataManager.Instance.RefreshData();
    }

    public void OnClickSpin()
    {
        if (isUiActive) return;

        if (DataManager.Instance != null && DataManager.Instance.SpendShells(5))
        {
            isUiActive = true;
            spinText.SetActive(false);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.shellClip);
            }

            StartCoroutine(AnimateBall());
        }
    }

    IEnumerator AnimateBall()
    {
        CollectableItem wonItem = null;

        if (allFishes.Count > 0 && allDecors.Count > 0)
        {
            if (Random.value < 0.70f) wonItem = allFishes[Random.Range(0, allFishes.Count)];
            else wonItem = allDecors[Random.Range(0, allDecors.Count)];
        }
        else if (allFishes.Count > 0)
        {
            wonItem = allFishes[Random.Range(0, allFishes.Count)];
        }
        else if (allDecors.Count > 0)
        {
            wonItem = allDecors[Random.Range(0, allDecors.Count)];
        }
        else
        {
            Debug.LogError("ERREUR : Aucune fiche (Item) dans les listes AllFishes et AllDecors !");
            isUiActive = false;
            yield break;
        }

        int randomIndex = Random.Range(0, ballPrefabs.Length);
        currentBall = Instantiate(ballPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        Canvas ballCanvas = currentBall.GetComponent<Canvas>();
        if (ballCanvas == null) ballCanvas = currentBall.AddComponent<Canvas>();

        ballCanvas.overrideSorting = true;
        ballCanvas.sortingOrder = 100;

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        float elapsed = 0;
        float duration = 1.2f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            currentBall.transform.position = Vector3.Lerp(spawnPoint.position, centerScreenPos.position, t);
            currentBall.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 3f, t);
            currentBall.transform.Rotate(0, 0, 400 * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Fin de l'animation, tentative d'affichage du panel...");

        int dejaDebloque = PlayerPrefs.GetInt("Unlocked_" + wonItem.itemID, 0);
        bool isNew = (dejaDebloque == 0);

        Debug.Log("Item ID: " + wonItem.itemID + " | Est nouveau ? " + isNew);

        if (resultNameText != null)
        {
            resultNameText.text = GetTranslatedItemName(wonItem.itemID);
        }

        if (resultImage != null)
        {
            resultImage.sprite = wonItem.itemSprite;
        }

        if (prizePanel != null)
        {
            prizePanel.SetActive(true);

            if (isNew)
            {
                if (newTextObject != null) newTextObject.SetActive(true);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverClip);
                }
            }
            else
            {
                if (newTextObject != null) newTextObject.SetActive(false);
            }

            PlayerPrefs.SetInt("Unlocked_" + wonItem.itemID, 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("La case 'Prize Panel' est VIDE !");
        }
    }


    public string GetTranslatedItemName(string itemID)
    {
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
        switch (itemID)
        {
            case "fish_blue": return lang == 0 ? "Blue Tang" : lang == 1 ? "擬刺尾鯛" : "Chirurgien Bleu";
            case "fish_clown": return lang == 0 ? "Clownfish" : lang == 1 ? "小丑魚" : "Poisson-Clown";
            case "fish_diodon": return lang == 0 ? "Diodon" : lang == 1 ? "刺河豚" : "Poisson-Globe";
            case "fish_mask": return lang == 0 ? "Masked Bannerfish" : lang == 1 ? "馬夫魚" : "Poisson-Cocher";
            case "fish_yellow": return lang == 0 ? "Yellow Tang" : lang == 1 ? "黃高鰭刺尾魚" : "Chirurgien Jaune";
            case "plant_anemone": return lang == 0 ? "Sea Anemone" : lang == 1 ? "海葵" : "Anémone";
            case "plant_coral": return lang == 0 ? "Coral" : lang == 1 ? "珊瑚" : "Corail";
            case "plant_seaweed": return lang == 0 ? "Seaweed" : lang == 1 ? "海藻" : "Algue";
            case "plant_rock": return lang == 0 ? "Rock" : lang == 1 ? "岩石" : "Rocher";
            default: return itemID;
        }
    }

    void UnlockItem(CollectableItem item)
    {
        PlayerPrefs.SetInt("Unlocked_" + item.itemID, 1);
        PlayerPrefs.Save();
        Debug.Log("Débloqué : " + item.itemName);
    }

    public void ClosePrizePanel()
    {
        if (currentBall != null) Destroy(currentBall);
        if (newTextObject != null) newTextObject.SetActive(false);
        prizePanel.SetActive(false);
        spinText.SetActive(true);
        isUiActive = false;
    }
}
