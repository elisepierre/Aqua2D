using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Musiques de Fond")]
    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip backgroundMusic;
    public AudioClip catchBackgroundMusic;
    public AudioClip endlessBackgroundMusic;

    [Header("Effets Sonores (SFX)")]
    public AudioSource sfxSource;
    public AudioClip whackClip;
    public AudioClip shellClip;
    public AudioClip trashClip;
    public AudioClip gameOverClip;
    public AudioClip pauseClip;
    public AudioClip homeClip;
    public AudioClip sirenSadClip;
    public AudioClip rockHitClip;

    [Header("Musiques de Fond")]
    public AudioClip aquariumMusic;

    [Header("Effets Sonores (SFX)")]
    public AudioClip crankClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Doublon détecté et supprimé : " + gameObject.name);
            Destroy(gameObject);
            return;
        }
    }

    public void RestartCurrentMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.Play();
        }
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlayAquariumMusic()
    {
        PlayMusic(aquariumMusic);
    }

    public void PlayWhackMusic()
    {
        musicSource.Stop();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("Musique Whack lancée sur : " + musicSource.gameObject.name);
    }

    public void PlayCatchMusic()
    {
        PlayMusic(catchBackgroundMusic);
    }

    public void PlayEndlessMusic()
    {
        PlayMusic(endlessBackgroundMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("AudioManager : Nouvelle musique lancée -> " + clip.name);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null) { Debug.LogError("SFX Source manquante !"); return; }
        if (clip == null) { Debug.LogError("AudioClip manquant dans l'appel !"); return; }

        sfxSource.PlayOneShot(clip);
        Debug.Log("Son joué : " + clip.name);
    }

    public void PlayCrankSound()
    {
        PlaySFX(crankClip);
    }
}
