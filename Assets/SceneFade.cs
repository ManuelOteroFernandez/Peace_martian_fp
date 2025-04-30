using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class SceneFader : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private static SceneFader instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void FadeOutAndLoad(int sceneIndex) {
        StartCoroutine(FadeOutIn(sceneIndex));
    }

    IEnumerator FadeOutIn(int sceneIndex) {
        yield return StartCoroutine(Fade(0, 1));
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator Fade(float from, float to) {
        float t = 0f;
        while (t < fadeDuration) {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
