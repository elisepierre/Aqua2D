using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BubbleClickManager : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public Image bubbleImage;

    private bool isPopping = false;

    public void OnBubbleClicked()
    {
        if (!isPopping)
        {
            StartCoroutine(PopSequence());
        }
    }

    IEnumerator PopSequence()
    {
        isPopping = true;
        anim.SetTrigger("PopNow");
        yield return new WaitForSeconds(0.4f);
        bubbleImage.enabled = false;
        yield return new WaitForSeconds(1.0f);
        bubbleImage.enabled = true;
        anim.SetTrigger("RegenNow");

        isPopping = false;
    }
}