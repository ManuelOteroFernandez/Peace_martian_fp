using UnityEngine;
using UnityEngine.EventSystems;

public class GetFocus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

}
