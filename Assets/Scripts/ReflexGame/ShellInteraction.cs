using UnityEngine;

public class ShellInteraction : MonoBehaviour
{
    private bool wasCollected = false;

    private void Collect()
    {
        if (wasCollected || Time.timeScale == 0) return;
        wasCollected = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        GetComponent<Collider2D>().enabled = false;

        CollectAnimation animScript = GetComponent<CollectAnimation>();
        if (animScript != null)
        {
            animScript.enabled = true;
            animScript.StartAnimation(GameManager.instance.globalScoreIcon);
        }
        GameManager.instance.AddScore(1);
    }

    void OnMouseDown() => Collect();
    void OnMouseEnter() { if (Input.GetMouseButton(0)) Collect(); }
}