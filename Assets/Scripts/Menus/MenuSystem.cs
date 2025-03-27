using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    /*
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Opciones()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Inicio()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void OpcionesAJugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    */

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
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
