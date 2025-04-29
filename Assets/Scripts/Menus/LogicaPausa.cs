using UnityEngine;

public class LogicaPausa : MonoBehaviour
{
    public GameObject ManuPause;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarJuego();
        }
    }

    public void PausarJuego()
    {
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
