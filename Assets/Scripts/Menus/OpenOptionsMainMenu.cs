using UnityEngine;

public class OpenOptionsMainMenu : MonoBehaviour
{
    LogicaEntreEscenas entreEscenas;

    // Start is called before the first frame update
    void Start()
    {
        entreEscenas = GameObject.FindGameObjectWithTag("EntreEscenas").GetComponent<LogicaEntreEscenas>();
    }

    public void OpenOptionPanel()
    {
        if (entreEscenas == null){
            Debug.LogError("MainMenu error: entreEscenas == null");
            return;
        }

        entreEscenas.SetActiveOptionMenu(gameObject,true);
        gameObject.SetActive(false);

    }
}
