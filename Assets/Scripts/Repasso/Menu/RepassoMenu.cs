using UnityEngine;

public class RepassoMenu : MonoBehaviour
{
    [SerializeField] GameObject panelMenu;

    public void Play()
    {
        panelMenu.SetActive(false);
    }

}
