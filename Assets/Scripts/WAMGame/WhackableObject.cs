using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WhackableObject : MonoBehaviour
{
    public bool isBadGuy;
    public float activeTime = 3f;
    public float moveDistance = 2f;
    public float moveSpeed = 5f;

    [Header("Animation Score")]
    public GameObject collectAnimationPrefab;
    public RectTransform scoreIconRect;

    public int currentHoleIndex = -1;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool wasHit = false;

    [Header("Vfx Whack")]
    public Sprite dizzySprite;
    public GameObject hammerPrefab;
    public float delayBeforeDestroy = 0.4f;

    void OnEnable()
    {
        if (WhackManager.Instance != null)
        {
            activeTime = WhackManager.Instance.globalActiveTime;
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
            if (currentHoleIndex != -1) WhackManager.Instance.ReleaseHole(currentHoleIndex);

            if (isBadGuy)
            {
                WhackManager.Instance.GameOver();
            }
            else
            {
                yield return Move(targetPos, startPos);
                Destroy(gameObject);
            }
        }
    }

    void OnMouseDown()
    {
        if (wasHit || Time.timeScale == 0) return;
        wasHit = true;

        if (currentHoleIndex != -1) WhackManager.Instance.ReleaseHole(currentHoleIndex);

        if (hammerPrefab != null)
        {
            Vector3 hammerPos = transform.position + new Vector3(-0.4f, 0.9f, -1f);
            GameObject hammer = Instantiate(hammerPrefab, hammerPos, Quaternion.identity);
            hammer.transform.SetParent(null);
            hammer.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }

        if (dizzySprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = dizzySprite;
        }

        if (isBadGuy)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.whackClip);
            StartCoroutine(HitAndDisappear());
        }
        else
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.shellClip);
            WhackManager.Instance.AddScore(1);

            if (collectAnimationPrefab != null)
            {
                GameObject animObj = Instantiate(collectAnimationPrefab, transform.position, Quaternion.identity);
                animObj.GetComponent<CollectAnimation>().StartAnimation(WhackManager.Instance.globalScoreIcon);
            }

            Destroy(gameObject);
        }
    }

    IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    IEnumerator HitAndDisappear()
    {
        transform.position += new Vector3(0.1f, 0, 0);
        yield return new WaitForSeconds(0.05f);
        transform.position -= new Vector3(0.1f, 0, 0);

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);
    }
}