using UnityEngine;
using System.Collections;

public class SirenObject : MonoBehaviour
{
    public float activeTime = 4f;
    public float moveSpeed = 5f;
    public float moveDistance = 2f;

    [Header("Sprites Sirène")]
    public Sprite sadSprite;

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
        if (WhackManager.Instance != null)
        {
            activeTime = WhackManager.Instance.globalActiveTime + 1f;
        }

        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
        wasHit = false;
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        yield return Move(startPos, targetPos);

        float timer = 0;
        while (timer < activeTime && !wasHit)
        {
            timer += Time.deltaTime;
            yield return null;
        }

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
        transform.position += new Vector3(0.05f, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        transform.position -= new Vector3(0.05f, 0f, 0f);

        yield return new WaitForSeconds(0.4f);

        if (currentHoleIndex != -1) WhackManager.Instance.ReleaseHole(currentHoleIndex);
        WhackManager.Instance.GameOver();
    }

    IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = 0;
        while (t < 1)
        {
            if (wasHit) yield break;
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }
}
