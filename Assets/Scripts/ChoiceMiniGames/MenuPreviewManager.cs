using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPreviewManager : MonoBehaviour
{
    private enum PreviewState { None, Clean, Swim, Whack }
    private PreviewState currentState = PreviewState.None;

    [Header("Clean Ocean Preview")]
    public MonoBehaviour parallaxEffect;
    public Animator HookAnimator;
    public Animator Trash1Animator;
    public Animator Trash2Animator;
    public GameObject playButtonClean;

    [Header("Swim Rush Preview")]
    public MonoBehaviour scrollEffect;
    public Animator fishAnimator;
    public Animator rockAnimator;
    public GameObject playButtonSwim;

    [Header("Fisher Whack Preview")]
    public GameObject fisherContainer;
    public GameObject playButtonWhack;

    void Start()
    {
        ResetAllPreviews();
    }

    public void OnClickCleanOcean()
    {
        if (currentState == PreviewState.Clean) return;
        ResetAllPreviews();
        currentState = PreviewState.Clean;

        if (parallaxEffect != null) parallaxEffect.enabled = true;

        if (HookAnimator != null)
        {
            HookAnimator.ResetTrigger("StopDemo");
            HookAnimator.SetTrigger("StartDemo");
        }
        if (Trash1Animator != null)
        {
            Trash1Animator.ResetTrigger("StopDemo");
            Trash1Animator.SetTrigger("StartDemo");
        }
        if (Trash2Animator != null)
        {
            Trash2Animator.ResetTrigger("StopDemo");
            Trash2Animator.SetTrigger("StartDemo");
        }
        playButtonClean.SetActive(true);
    }

    public void OnClickSwimRush()
    {
        if (currentState == PreviewState.Swim) return;

        ResetAllPreviews();
        currentState = PreviewState.Swim;

        if (scrollEffect != null) scrollEffect.enabled = true;

        if (fishAnimator != null)
        {
            fishAnimator.ResetTrigger("StopDemo");
            fishAnimator.SetTrigger("StartDemo");
        }

        if (rockAnimator != null)
        {
            rockAnimator.ResetTrigger("StopDemo");
            rockAnimator.SetTrigger("StartDemo");
        }

        playButtonSwim.SetActive(true);
    }
    public void OnClickFisherWhack()
    {
        if (currentState == PreviewState.Whack) return;

        ResetAllPreviews();
        currentState = PreviewState.Whack;

        if (fisherContainer != null) fisherContainer.SetActive(true);
        playButtonWhack.SetActive(true);
    }

    private void ResetAllPreviews()
    {
        currentState = PreviewState.None;

        if (parallaxEffect != null) parallaxEffect.enabled = false;
        if (scrollEffect != null) scrollEffect.enabled = false;

        if (fisherContainer != null) fisherContainer.SetActive(false);

        if (playButtonClean != null) playButtonClean.SetActive(false);
        if (playButtonSwim != null) playButtonSwim.SetActive(false);
        if (playButtonWhack != null) playButtonWhack.SetActive(false);

        if (HookAnimator != null) HookAnimator.SetTrigger("StopDemo");
        if (Trash1Animator != null) Trash1Animator.SetTrigger("StopDemo");
        if (Trash2Animator != null) Trash2Animator.SetTrigger("StopDemo");
        if (fishAnimator != null) fishAnimator.SetTrigger("StopDemo");
        if (rockAnimator != null) rockAnimator.SetTrigger("StopDemo");
    }

    public void LoadReflexGame() => SceneManager.LoadScene("ReflexGameScene");
    public void LoadEndlessGame() => SceneManager.LoadScene("EndlessGameScene");
    public void LoadWAMGame() => SceneManager.LoadScene("WAMGameScene");
}