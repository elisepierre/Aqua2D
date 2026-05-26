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
    public List<CollectableItem> allFishes;  // Glisse tes poissons ici
    public List<CollectableItem> allDecors;  // Glisse tes décors ici

    [Header("Animation")]
    public GameObject[] ballPrefabs;
    public Transform spawnPoint;
    public Transform centerScreenPos;

    private GameObject currentBall;
    [HideInInspector] public bool isUiActive = false;

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
            StartCoroutine(AnimateBall());
        }
    }

    IEnumerator AnimateBall()
    {
        CollectableItem wonItem = null;

        // --- SYSTÈME DE TIRAGE SÉCURISÉ ---
        if (allFishes.Count > 0 && allDecors.Count > 0)
        {
            // Si les deux listes sont remplies, on fait le 70/30 normal
            if (Random.value < 0.70f) wonItem = allFishes[Random.Range(0, allFishes.Count)];
            else wonItem = allDecors[Random.Range(0, allDecors.Count)];
        }
        else if (allFishes.Count > 0)
        {
            // Si tu n'as QUE des poissons, on donne un poisson d'office
            wonItem = allFishes[Random.Range(0, allFishes.Count)];
        }
        else if (allDecors.Count > 0)
        {
            // Si tu n'as QUE des décors, on donne un décor
            wonItem = allDecors[Random.Range(0, allDecors.Count)];
        }
        else
        {
            // Si TOUT est vide, on arrête avant de crash
            Debug.LogError("ERREUR : Aucune fiche (Item) dans les listes AllFishes et AllDecors !");
            isUiActive = false;
            yield break;
        }

        // --- CRÉATION BOULE ---
        int randomIndex = Random.Range(0, ballPrefabs.Length);
        currentBall = Instantiate(ballPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // --- FIX CANVAS ---
        Canvas ballCanvas = currentBall.GetComponent<Canvas>();
        if (ballCanvas == null) ballCanvas = currentBall.AddComponent<Canvas>();

        ballCanvas.overrideSorting = true;
        ballCanvas.sortingOrder = 100;

        // --- FIX PHYSIQUE ---
        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        // --- ANIMATION ---
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

        // --- AFFICHAGE DU RÉSULTAT (Ligne 74 - Zone du crash) ---
        Debug.Log("Fin de l'animation, tentative d'affichage du panel...");

        if (resultNameText != null) resultNameText.text = wonItem.itemName;
        else Debug.LogError("La case 'Result Name Text' est VIDE dans l'inspecteur !");

        if (resultImage != null) resultImage.sprite = wonItem.itemSprite;
        else Debug.LogError("La case 'Result Image' est VIDE dans l'inspecteur !");

        if (prizePanel != null)
        {
            prizePanel.SetActive(true);
            // On sauvegarde ici
            PlayerPrefs.SetInt("Unlocked_" + wonItem.itemID, 1);
            PlayerPrefs.Save();
        }
        else Debug.LogError("La case 'Prize Panel' est VIDE dans l'inspecteur !");
    }

    void UnlockItem(CollectableItem item)
    {
        // On sauvegarde le fait qu'il est débloqué (1 = Vrai)
        PlayerPrefs.SetInt("Unlocked_" + item.itemID, 1);
        PlayerPrefs.Save();
        Debug.Log("Débloqué : " + item.itemName);
    }

    public void ClosePrizePanel()
    {
        if (currentBall != null) Destroy(currentBall);
        prizePanel.SetActive(false);
        spinText.SetActive(true);
        isUiActive = false;
    }
}