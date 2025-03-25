using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class TestingUtils : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField] int targetFps;
    
    private void Awake() {
        //Application.targetFrameRate = targetFps;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown("1")) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }

        if (Input.GetKeyDown("2")) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }

        if (Input.GetKeyDown("3")) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
        }
    }
}
