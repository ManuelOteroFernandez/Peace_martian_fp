using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LogicaVolumenMod : MonoBehaviour
{
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderEfectos;

    public float volumenGeneral;
    public float volumenMusica;
    public float volumenEfectos;

    public Image imagenMute;

    public AudioMixer mixer;

    void Start()
    {
        volumenGeneral = PlayerPrefs.GetFloat("volumenGeneral", 0.5f);
        volumenMusica = PlayerPrefs.GetFloat("volumenMusica", 0.5f);
        volumenEfectos = PlayerPrefs.GetFloat("volumenEfectos", 0.5f);

        sliderGeneral.value = volumenGeneral;
        sliderMusica.value = volumenMusica;
        sliderEfectos.value = volumenEfectos;

        CambiarVolumenGeneral(volumenGeneral);
        CambiarVolumenMusica(volumenMusica);
        CambiarVolumenEfectos(volumenEfectos);
    }

    public void CambiarVolumenGeneral(float valor)
    {
        volumenGeneral = valor;
        PlayerPrefs.SetFloat("volumenGeneral", valor);
        mixer.SetFloat("VolumenGeneral", Mathf.Log10(valor) * 20);
        RevisarSiEstoyMute();
    }

    public void CambiarVolumenMusica(float valor)
    {
        volumenMusica = valor;
        PlayerPrefs.SetFloat("volumenMusica", valor);
        mixer.SetFloat("VolumenMusica", Mathf.Log10(valor) * 20);
    }

    public void CambiarVolumenEfectos(float valor)
    {
        volumenEfectos = valor;
        PlayerPrefs.SetFloat("volumenEfectos", valor);
        mixer.SetFloat("VolumenEfectos", Mathf.Log10(valor) * 20);
    }

    public void RevisarSiEstoyMute()
    {
        imagenMute.enabled = volumenGeneral <= 0.0001f;
    }
}
