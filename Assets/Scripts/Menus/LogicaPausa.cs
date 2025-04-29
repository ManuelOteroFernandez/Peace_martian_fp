using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaPausa : MonoBehaviour
{
    public GameObject ManuPause;

    public void PausarJuego()
    {
        // No se puede pausar en el menu de inicio
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ManuPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            ManuPause.SetActive(false);
        }

    }
}
