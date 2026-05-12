using UnityEngine;
using System.Collections;

public class CollectAnimation : MonoBehaviour
{
    [SerializeField] private float duration = 0.6f;
    private SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    public void StartAnimation(RectTransform targetIcon)
    {
        transform.SetParent(null);
        StartCoroutine(MoveAndFade(targetIcon));
    }

    IEnumerator MoveAndFade(RectTransform targetIcon)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, targetIcon.position);
            Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

            transform.position = Vector3.Lerp(startPos, worldTarget, t);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.3f, t);

            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                sr.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}