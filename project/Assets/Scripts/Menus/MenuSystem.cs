using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionTextOne;
    [SerializeField] private TextMeshProUGUI versionTextTwo;

    void Start() {
        versionTextOne.text = $"Versión {Application.version}";
        versionTextTwo.text = $"Versión {Application.version}";
    }
    
    public void Jugar()
    {
        LevelManager.RestoreLastPlay();
    }

    public void ContinuarSiguienteNivel()
    {
        LevelManager.NextLevel();
    }

    public void Opciones()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void NuevaPartida()
    {
        LevelManager.StartPlay();
        gameObject.SetActive(false);
    }

    public void OpcionesAJugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    

    public void CambiarEscenaInicio()
    {
        SceneManager.LoadScene(0);
    }

    public void CambiarEscenaJuego()
    {
        SceneManager.LoadScene(1);
    }


    public void Salir()
    {
        // Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
