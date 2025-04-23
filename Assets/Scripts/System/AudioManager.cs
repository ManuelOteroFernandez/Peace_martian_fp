using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] public AudioSource introSource;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip menuTheme1;
    [SerializeField] public AudioClip menuTheme2;
    [SerializeField] public AudioClip level1Theme;
    [SerializeField] public AudioClip level2Theme;
    [SerializeField] public AudioClip level3Theme;

    [Header("Pitch Randomization")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.2f;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip, bool randomPitch = false) {
        if (clip == null) {
            return;
        }

        if (randomPitch) {
            sfxSource.pitch = Random.Range(minPitch, maxPitch);
        } else {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, bool loop = true) {
        if (audioSource.clip == clip){
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayMusicMainMenu() {
        double startTime = AudioSettings.dspTime + 0.1f;
        double introDuration = (double)menuTheme1.samples / menuTheme1.frequency;

        introSource.clip = menuTheme1;
        introSource.PlayScheduled(startTime);

        audioSource.clip = menuTheme2;
        audioSource.loop = true;
        audioSource.PlayScheduled(startTime + introDuration);
    }

    public void StopMusic() {
        audioSource.Stop();
    }

    public void PauseMusic() {
        audioSource.Pause();
    }

    public void ResumeMusic() {
        audioSource.UnPause();
    }
}
