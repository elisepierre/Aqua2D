using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPreviewManager : MonoBehaviour
{
    private enum PreviewState { None, Clean, Swim, Whack }
    private PreviewState currentState = PreviewState.None;

    [Header("Clean Ocean Preview")]
    public MonoBehaviour parallaxEffect;
    public Animator cleanOceanAnimator;
    public GameObject playButtonClean;

    [Header("Swim Rush Preview")]
    public MonoBehaviour scrollEffect;
    public Animator fishAnimator;
    public Animator rockAnimator;
    public GameObject playButtonSwim;

    [Header("Fisher Whack Preview")]
    public Animator fisherWhackAnimator;
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

        if (cleanOceanAnimator != null)
        {
            cleanOceanAnimator.ResetTrigger("StopDemo");
            cleanOceanAnimator.SetTrigger("StartDemo");
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

        if (fisherWhackAnimator != null)
        {
            fisherWhackAnimator.ResetTrigger("StopDemo");
            fisherWhackAnimator.SetTrigger("StartDemo");
        }
        playButtonWhack.SetActive(true);
    }

    private void ResetAllPreviews()
    {
        currentState = PreviewState.None;

        if (parallaxEffect != null) parallaxEffect.enabled = false;
        if (scrollEffect != null) scrollEffect.enabled = false;

        if (playButtonClean != null) playButtonClean.SetActive(false);
        if (playButtonSwim != null) playButtonSwim.SetActive(false);
        if (playButtonWhack != null) playButtonWhack.SetActive(false);

        if (cleanOceanAnimator != null) cleanOceanAnimator.SetTrigger("StopDemo");
        if (fishAnimator != null) fishAnimator.SetTrigger("StopDemo");
        if (rockAnimator != null) rockAnimator.SetTrigger("StopDemo");
        if (fisherWhackAnimator != null) fisherWhackAnimator.SetTrigger("StopDemo");
    }

    public void LoadReflexGame()
    {
        StopMusicBeforeLoading();
        SceneManager.LoadScene("ReflexGameScene");
    }

    public void LoadEndlessGame()
    {
        StopMusicBeforeLoading();
        SceneManager.LoadScene("EndlessGameScene");
    }

    public void LoadWAMGame()
    {
        StopMusicBeforeLoading();
        SceneManager.LoadScene("WAMGameScene");
    }

    // Petite fonction utilitaire pour éviter de répéter le code
    private void StopMusicBeforeLoading()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
    }
}