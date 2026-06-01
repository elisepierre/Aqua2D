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

    [Header("Musiques de Fond")]
    public AudioClip aquariumMusic; // Glisse ici une musique zen/calme

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
            // SI UN DOUBLON ARRIVE : ON LE DÉTRUIT TOUT DE SUITE
            Debug.Log("Doublon détecté et supprimé : " + gameObject.name);
            Destroy(gameObject);
            return; // On arrête l'exécution ici pour ne pas lancer le reste du script
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

    // Ajoute cette fonction pour la lancer
    public void PlayAquariumMusic()
    {
        PlayMusic(aquariumMusic);
    }

    // Lance la musique du jeu Whack-a-Fish
    public void PlayWhackMusic()
    {
        musicSource.Stop(); // Arrête tout net
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("Musique Whack lancée sur : " + musicSource.gameObject.name);
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
        if (musicSource == null || clip == null) return;

        // Si c'est déjà le même clip qui joue, on ne touche à rien
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop(); // ON FORCE L'ARRÊT
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