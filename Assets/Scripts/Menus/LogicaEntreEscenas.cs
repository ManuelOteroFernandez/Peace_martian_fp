using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LogicaEntreEscenas : MonoBehaviour
{
    public GameObject MenuOpciones;
    public GameObject VictoryMenu;
    public GameObject DefeatMenu;

    SceneFader fader;

    private void Awake()
    {
        var noDestruirEntreEscenas = FindObjectsByType<LogicaEntreEscenas>(FindObjectsSortMode.None);
        if (noDestruirEntreEscenas.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        fader = FindFirstObjectByType<SceneFader>();
    }

    public void SetActiveOptionMenu(GameObject LastMenu,bool value)
    {
        if(MenuOpciones.IsUnityNull()){
            Debug.LogError("LogicaEntreEscenas error: MenuOpciones == null");
            return;
        }

        MenuOpciones.SetActive(value);
        OptionsReturn optionsRet = MenuOpciones.GetComponent<OptionsReturn>();
        optionsRet.SetLastMenu(LastMenu);

    }

    
    public void SetActiveVictoryMenu(bool value)
    {
        if(VictoryMenu.IsUnityNull()){
            Debug.LogError("LogicaEntreEscenas error: VictoryMenu == null");
            return;
        }
        StartCoroutine(ActiveVictoryMenuCoroutune(value));
    }

    IEnumerator ActiveVictoryMenuCoroutune(bool value){

        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();
        VictoryMenu.SetActive(value);
        yield return fader.FadeOut();
    }

    
    public void SetActiveDefeatMenu(bool value)
    {
        if(DefeatMenu.IsUnityNull()){
            Debug.LogError("LogicaEntreEscenas error: DefeatMenu == null");
            return;
        }

        StartCoroutine(ActiveDefeatMenuCoroutune(value));

    }

     IEnumerator ActiveDefeatMenuCoroutune(bool value){

        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();
        DefeatMenu.SetActive(value);
        yield return fader.FadeOut();
    }

}
