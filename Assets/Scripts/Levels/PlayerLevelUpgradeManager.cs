using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLevelUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject levelUpgradeUI;
    [SerializeField] private TMP_Text[] optionTexts;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private List<PlayerUpgradeSO> upgrades;

    private PlayerManager playerManager;
    private List<PlayerUpgradeSO> availableUpgrades;

    private void Start()
    {
        //playerManager = GetComponent<PlayerManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        availableUpgrades = new List<PlayerUpgradeSO>(upgrades);
        levelUpgradeUI.SetActive(false); 

        Debug.Log("PlayerLevelUpgradeManager started.");

        // Subscribe to the level-up event
        LevelingSystem.OnLevelUp += ShowLevelUpOptions;
    }
    private void OnDestroy()
    {
        // Unsubscribe from the level-up event to prevent memory leaks
        LevelingSystem.OnLevelUp -= ShowLevelUpOptions;
    }

    public void ShowLevelUpOptions()
    {
        Debug.Log("ShowLevelUpOptions called.");
        GameManager.instance.PauseGame();
        levelUpgradeUI.SetActive(true);
        //DisplayRandomUpgrades();
        DisplayUpgrades();
    }
    private void DisplayRandomUpgrades()
    {
        Debug.Log("Displaying random upgrades.");
        for(int i = 0; i < optionButtons.Length; i++)
        {
            if (availableUpgrades.Count == 0) break;

            int randomIndex = Random.Range(0, availableUpgrades.Count);
            PlayerUpgradeSO selectedUpgrade = availableUpgrades[randomIndex];

            optionTexts[i].text = $"{selectedUpgrade.upgradeName}: {selectedUpgrade.upgradeDescription}";
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ApplyUpgrade(selectedUpgrade));
            optionButtons[i].onClick.AddListener(HideLevelUpOptions);

            availableUpgrades.RemoveAt(randomIndex);
        }
    }
    private void DisplayUpgrades()
    {
        Debug.Log("Displaying upgrades.");
        for (int i = 0; i < optionButtons.Length && i < availableUpgrades.Count; i++)
        {
            PlayerUpgradeSO upgrade = availableUpgrades[i];

            optionTexts[i].text = $"{upgrade.upgradeName}: {upgrade.upgradeDescription}";
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ApplyUpgrade(upgrade));
            optionButtons[i].onClick.AddListener(HideLevelUpOptions);
        }
    }

    private void ApplyUpgrade(PlayerUpgradeSO upgrade)
    {
        Debug.Log("Applying upgrade: " + upgrade.upgradeName);

        if (upgrade == null)
        {
            Debug.LogError("Upgrade is null");
            return;
        }
        switch (upgrade.upgradeType)
        {
            case PlayerUpgradeSO.UpgradeType.MaxHealth:
                playerManager.IncreaseMaxHealth(Mathf.RoundToInt(upgrade.effectStrength));
                break;
            case PlayerUpgradeSO.UpgradeType.HealthRegen:
                //playerManager.IncreaseHealthRegen(upgrade.effectStrength);
                if (playerManager != null)
                {
                    // Directly apply the upgrade to the Health component
                    Health health = playerManager.GetComponent<Health>();
                    health.ApplyUpgrade(upgrade);
                }
                break;
            case PlayerUpgradeSO.UpgradeType.MovementSpeed:
                playerManager.IncreaseMovementSpeed(upgrade.effectStrength);
                break;
            case PlayerUpgradeSO.UpgradeType.DashRange:
                playerManager.IncreaseDashSpeed(upgrade.effectStrength);
                break;
            case PlayerUpgradeSO.UpgradeType.DashCooldown:
                playerManager.ReduceDashCooldown(upgrade.effectStrength);
                break;
        }

        HideLevelUpOptions();
    }

    private void HideLevelUpOptions()
    {
        Debug.Log("Hiding level-up options.");
        levelUpgradeUI.SetActive(false);
        GameManager.instance.ResumeGame();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            ShowLevelUpOptions();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            HideLevelUpOptions();
        }
    }
}
