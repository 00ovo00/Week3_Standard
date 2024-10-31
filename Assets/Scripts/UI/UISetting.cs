using UnityEngine;

public class UISetting : MonoBehaviour
{
    public GameObject SettingMenu;
    
    private PlayerController controller;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.setting += Toggle;

        SettingMenu.SetActive(false);
    }
    
    public void Toggle()
    {
        if (SettingMenu.activeInHierarchy)
        {
            SettingMenu.SetActive(false);
        }
        else
        {
            SettingMenu.SetActive(true);
        }
    }
}