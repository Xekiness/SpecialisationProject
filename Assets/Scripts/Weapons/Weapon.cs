using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData; // Weapon data like damage, fire rate, etc.
    protected float nextAttackTime = 0f;

    // Event for ammo change, to update the UI
    public UnityEvent OnAmmoChanged;

    protected virtual void Awake()
    {
        if (OnAmmoChanged == null)
        {
            OnAmmoChanged = new UnityEvent();
        }
    }
    private void Start()
    {
        InitializeWeapon();
    }

    public int CurrentAmmo { get; protected set; } // Current ammo in the magazine

    private void InitializeWeapon()
    {
        // Only initialize ammo if maxAmmo is set (for ranged weapons)
        if (weaponData.magazineSize > 0)
        {
            CurrentAmmo = weaponData.magazineSize; // Initialize magazine count
        }

        // Trigger initial ammo update
        OnAmmoChanged.Invoke();
    }

    public abstract void Attack(Vector2 targetPosition);

    public virtual void Equip()
    {
        gameObject.SetActive(true);
        // Additional equip logic, if needed
    }

    public virtual void Unequip()
    {
        gameObject.SetActive(false);
        // Additional unequip logic, if needed
    }

    public virtual void Reload()
    {
        if (weaponData.magazineSize > 0 && CurrentAmmo < weaponData.magazineSize)
        {
            CurrentAmmo += weaponData.magazineSize;

            //int neededAmmo = weaponData.maxAmmo - CurrentAmmo;
            //int ammoToReload = Mathf.Min(neededAmmo, ReserveAmmo);
            //CurrentAmmo += ammoToReload;
            //ReserveAmmo -= ammoToReload;

            // Trigger ammo change event
            OnAmmoChanged.Invoke();
        }
    }
    public abstract int GetCurrentAmmo();
    //public virtual void GetCurrentAmmo()
    //{
    //    return 
    //}
}
