using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones panelPausa;
    public ControladorOpciones panelOpciones;

    // Start is called before the first frame update
    void Start()
    {
        panelPausa = GameObject.FindGameObjectWithTag("opciones").GetComponent<ControladorOpciones>();
        panelOpciones = GameObject.FindGameObjectWithTag("pausa").GetComponent<ControladorOpciones>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        bool estaActivo = panelPausa.pantallaPausa.activeSelf;
        panelPausa.pantallaPausa.SetActive(!estaActivo);
    }
}
