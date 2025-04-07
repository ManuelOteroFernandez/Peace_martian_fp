using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    public ControladorOpciones panelOpciones;

    // Start is called before the first frame update
    void Start()
    {
        panelOpciones = GameObject.FindGameObjectWithTag("opciones").GetComponent<ControladorOpciones>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarOpciones();
        }
    }

    public void AlternarOpciones()
    {
        bool estaActivo = panelOpciones.pantallaOpciones.activeSelf;
        panelOpciones.pantallaOpciones.SetActive(!estaActivo);
    }
}
