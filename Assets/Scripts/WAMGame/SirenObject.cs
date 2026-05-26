using UnityEngine;
using System.Collections;

public class SirenObject : MonoBehaviour
{
    public float activeTime = 4f; // Elle reste plus longtemps pour bloquer le trou
    public float moveSpeed = 5f;
    public float moveDistance = 2f;

    public int currentHoleIndex = -1;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool wasHit = false;

    void OnEnable()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * moveDistance;
        wasHit = false;
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        // Montée
        yield return Move(startPos, targetPos);

        // Attente
        float timer = 0;
        while (timer < activeTime && !wasHit)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Si on ne l'a pas touchée (Bravo !)
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

        // PUNITIF : Si on touche la sirène, c'est Game Over
        Debug.Log("Tu as touché la sirène !");
        WhackManager.Instance.GameOver();
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
}