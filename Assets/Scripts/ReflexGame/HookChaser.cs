using UnityEngine;
using System.Collections;

public class HookChaser : MonoBehaviour
{
    public Transform hookHead;
    public float speed = 25f;
    private float topY = 4.5f;

    void Awake()
    {
        // 1. Auto-détection de la tête pour éviter les erreurs de lien
        if (hookHead == null)
        {
            hookHead = transform.Find("HookHead");
        }

        if (hookHead != null)
        {
            hookHead.gameObject.SetActive(false);
            Debug.Log("[DEBUG] HookHead trouvé et désactivé.");
        }
        else
        {
            Debug.LogError("[DEBUG] ERREUR : Impossible de trouver un enfant nommé 'HookHead'. Renomme ton objet enfant !");
        }
    }

    public void GrabTrashAtPosition(GameObject targetTrash)
    {
        if (targetTrash == null) return;
        StartCoroutine(FishingRoutine(targetTrash));
    }

    IEnumerator FishingRoutine(GameObject targetTrash)
    {
        Debug.Log("[DEBUG] Début de la pêche.");

        // Geler le déchet
        Rigidbody2D rb = targetTrash.GetComponent<Rigidbody2D>();
        if (rb != null) { rb.isKinematic = true; rb.velocity = Vector2.zero; }

        Vector3 trashPos = targetTrash.transform.position;
        hookHead.position = transform.position;
        hookHead.gameObject.SetActive(true);

        // Descente rapide
        while (hookHead.position.y > trashPos.y)
        {
            hookHead.position += new Vector3(0, -speed * Time.deltaTime, 0);
            yield return null;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.trashClip);
        }

        // Attraper
        targetTrash.transform.SetParent(hookHead);
        targetTrash.transform.localPosition = Vector3.zero;

        // Remontée rapide
        while (hookHead.position.y < topY)
        {
            hookHead.position += new Vector3(0, speed * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(targetTrash);
        Destroy(gameObject); // On supprime le crochet
    }
}