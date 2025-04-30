using UnityEngine;
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
    }
}
