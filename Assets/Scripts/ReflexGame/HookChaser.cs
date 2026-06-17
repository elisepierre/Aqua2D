using UnityEngine;
using System.Collections;

public class HookChaser : MonoBehaviour
{
    public Transform hookHead;
    public float speed = 25f;
    private float topY = 4.5f;

    void Awake()
    {
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

        Rigidbody2D rb = targetTrash.GetComponent<Rigidbody2D>();
        if (rb != null) { rb.isKinematic = true; rb.velocity = Vector2.zero; }

        Vector3 trashPos = targetTrash.transform.position;
        hookHead.position = transform.position;
        hookHead.gameObject.SetActive(true);

        while (hookHead.position.y > trashPos.y)
        {
            hookHead.position += new Vector3(0, -speed * Time.deltaTime, 0);
            yield return null;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.trashClip);
        }

        targetTrash.transform.SetParent(hookHead);
        targetTrash.transform.localPosition = Vector3.zero;

        while (hookHead.position.y < topY)
        {
            hookHead.position += new Vector3(0, speed * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(targetTrash);
        Destroy(gameObject);
    }
}
