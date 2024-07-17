using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanelUI;

    private void Start()
    {
        settingsPanelUI.SetActive(false);
    }
    public void ShowSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(true);
        }
    }
    public void HideSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(false);
        }
    }
}
