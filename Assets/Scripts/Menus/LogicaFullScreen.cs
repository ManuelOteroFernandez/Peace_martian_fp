using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicaFullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDropDown;
    Resolution[] resoluciones;

    // Start is called before the first frame update
    void Start()
    {
        // Cargar estado de pantalla completa desde PlayerPrefs
        if (PlayerPrefs.HasKey("pantallaCompleta"))
        {
            bool pantallaCompletaGuardada = PlayerPrefs.GetInt("pantallaCompleta") == 1;
            Screen.fullScreen = pantallaCompletaGuardada;
            toggle.isOn = pantallaCompletaGuardada;
        }
        else
        {
            // Si no hay un valor guardado, guardar el estado actual
            PlayerPrefs.SetInt("pantallaCompleta", Screen.fullScreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        RevisarResolucion();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;

        // Guardar estado de pantalla completa en PlayerPrefs
        PlayerPrefs.SetInt("pantallaCompleta", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        resolucionesDropDown.AddOptions(opciones);
        resolucionesDropDown.value = resolucionActual;
        resolucionesDropDown.RefreshShownValue();

        resolucionesDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);
        PlayerPrefs.Save();
        
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
}
