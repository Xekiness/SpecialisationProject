using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeSO")]
public class PlayerUpgradeSO : ScriptableObject
{
    public enum UpgradeType
    {
        MaxHealth,
        HealthRegen,
        MovementSpeed,
        DashRange,
        DashCooldown
    };

    public UpgradeType upgradeType;
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public float effectStrength;
}
