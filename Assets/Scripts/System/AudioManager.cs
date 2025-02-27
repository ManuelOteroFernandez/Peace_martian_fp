using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Pitch Randomization")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.2f;

    private void Awake()
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

    public void PlaySFX(AudioClip clip, bool randomPitch = false)
    {
        if (clip == null) return;

        if (randomPitch)
        {
            sfxSource.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip == null) return;

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // 🔥 Pausar y reanudar la música
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
}
