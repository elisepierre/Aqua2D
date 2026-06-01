using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Musiques de Fond")]
    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip backgroundMusic;      // Musique pour le Whack-a-Fish
    public AudioClip catchBackgroundMusic; // NOUVEAU : Musique différente pour le Deep Sea Catch
    public AudioClip endlessBackgroundMusic;

    [Header("Effets Sonores (SFX)")]
    public AudioSource sfxSource;
    public AudioClip whackClip;
    public AudioClip shellClip;        // Partagé : Récolte de coquillage
    public AudioClip trashClip;        // NOUVEAU : Quand le hook attrape un déchet
    public AudioClip gameOverClip;     // Partagé : Fin de partie
    public AudioClip pauseClip;        // Partagé : Bouton pause
    public AudioClip homeClip;         // Partagé : Bouton home
    public AudioClip sirenSadClip;
    public AudioClip rockHitClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        // Important : On ne relance pas la musique si elle joue déjà 
        // pour éviter les coupures entre le Menu et la Story
        PlayMusic(menuMusic);
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // Lance la musique du jeu Whack-a-Fish
    public void PlayWhackMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Lance la musique du jeu Deep Sea Catch
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
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return; // Déjà en cours

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}