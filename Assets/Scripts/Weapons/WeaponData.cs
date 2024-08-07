using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon Data", order = 51)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public WeaponRarity rarity;

    // These properties can be left null/0 for melee weapons and filled for ranged weapons
    public int magazineSize; //Only for ranged weapons
    public GameObject bulletPrefab; // Only for ranged weapons
    public float bulletSpeed; // Only for ranged weapons
}

public enum WeaponRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
