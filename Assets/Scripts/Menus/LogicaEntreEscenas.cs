using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LogicaEntreEscenas : MonoBehaviour
{
    public GameObject MenuOpciones;
    private void Awake()
    {
        var noDestruirEntreEscenas = FindObjectsOfType<LogicaEntreEscenas>();
        if (noDestruirEntreEscenas.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveOptionMenu(GameObject LastMenu,bool value)
    {
        if(MenuOpciones.IsUnityNull()){
            Debug.LogError("LogicaEntreEscenas error: ManuOpciones == null");
            return;
        }

        MenuOpciones.SetActive(value);
        OptionsReturn optionsRet = MenuOpciones.GetComponent<OptionsReturn>();
        optionsRet.SetLastMenu(LastMenu);

    }
}
