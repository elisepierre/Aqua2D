using UnityEngine;
using System.Collections;

public class SirenObject : MonoBehaviour
{
    public float activeTime = 4f;
    public float moveSpeed = 5f;
    public float moveDistance = 2f;

    [Header("Sprites Sirène")]
    public Sprite sadSprite; // Glisse le sprite de la sirène triste ici dans l'Inspecteur

    public int currentHoleIndex = -1;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool wasHit = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // On synchronise son temps sur la difficulté actuelle du manager
        if (WhackManager.Instance != null)
        {
            activeTime = WhackManager.Instance.globalActiveTime + 1f; // On lui laisse un peu plus de temps sur le reef
        }

        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
        wasHit = false;
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        // Montée du trou
        yield return Move(startPos, targetPos);

        // Attente hors du trou
        float timer = 0;
        while (timer < activeTime && !wasHit)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Si elle n'a pas été touchée (Bravo, l'enfant a suivi les règles !)
        if (!wasHit)
        {
            yield return Move(targetPos, startPos);
            if (currentHoleIndex != -1) WhackManager.Instance.ReleaseHole(currentHoleIndex);
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (wasHit || Time.timeScale == 0) return;
        wasHit = true;

        Debug.Log("Aïe ! La sirène a été touchée !");

        // JOUE LE SON TRISTE DE LA SIRÈNE ICI
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sirenSadClip);
        }

        if (sadSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = sadSprite;
        }

        StartCoroutine(SadBeforeGameOver());
    }

    IEnumerator SadBeforeGameOver()
    {
        // Petit effet de secousse pour simuler le coup
        transform.position += new Vector3(0.05f, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        transform.position -= new Vector3(0.05f, 0f, 0f);

        // On attend 0.4 seconde pour que l'enfant réalise sa bêtise en voyant la sirène triste
        yield return new WaitForSeconds(0.4f);

        // Libération du trou et déclenchement du GameOver historique
        if (currentHoleIndex != -1) WhackManager.Instance.ReleaseHole(currentHoleIndex);
        WhackManager.Instance.GameOver();
    }

    IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = 0;
        while (t < 1)
        {
            if (wasHit) yield break; // Arrête le mouvement si on se fait taper dessus
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }
}