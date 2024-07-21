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
        playerManager = GetComponent<PlayerManager>();
        availableUpgrades = new List<PlayerUpgradeSO>(upgrades);
    }
    public void ShowLevelUpOptions()
    {
        GameManager.instance.PauseGame();
        levelUpgradeUI.SetActive(true);
        DisplayRandomUpgrades();
    }
    private void DisplayRandomUpgrades()
    {
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

    private void ApplyUpgrade(PlayerUpgradeSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case PlayerUpgradeSO.UpgradeType.MaxHealth:
                playerManager.IncreaseMaxHealth(upgrade.effectStrength);
                break;
            case PlayerUpgradeSO.UpgradeType.HealthRegen:
                playerManager.IncreaseHealthRegen(upgrade.effectStrength);
                break;
            case PlayerUpgradeSO.UpgradeType.PickupRange:
                playerManager.IncreasePickupRange(upgrade.effectStrength);
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
    }

    private void HideLevelUpOptions()
    {
        levelUpgradeUI.SetActive(false);
        GameManager.instance.ResumeGame();
    }
}
