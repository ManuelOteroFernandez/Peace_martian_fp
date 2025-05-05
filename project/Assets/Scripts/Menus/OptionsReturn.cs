using UnityEngine;

public class OptionsReturn : MonoBehaviour
{
    GameObject LastMenu;

    public void Return() {
        if (LastMenu == null) {
            Debug.LogError("Back option menu error: LastMenu == null");
            return;
        }
        
        LastMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetLastMenu(GameObject LastMenu) {
        this.LastMenu = LastMenu;
    }
}
