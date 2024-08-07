using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons;
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;
    public UnityEvent<Weapon> OnWeaponSwitch;

    [SerializeField] private TMP_Text ammoText;

    private void Start()
    {
        if(OnWeaponSwitch == null)
        {
            OnWeaponSwitch = new UnityEvent<Weapon>();  
        }

        // Ensure weapons list is not null
        if (weapons == null)
        {
            weapons = new List<Weapon>();
        }

        // Find all weapons under this GameObject
        foreach (Transform child in transform)
        {
            Weapon weapon = child.GetComponent<Weapon>();
            if (weapon != null && !weapons.Contains(weapon)) //If weapon in list already, don't add
            {
                weapons.Add(weapon);
                weapon.gameObject.SetActive(false); // Deactivate all weapons initially
            }
        }
        if (weapons.Count > 0)
        {
            EquipWeapon(0);
            Debug.Log("WeaponManager Start - Equipped Weapon: " + currentWeapon);
        }
        else
        {
            Debug.LogWarning("Weapon list is empty or not assigned.");
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void HandleWeaponSwitching()
    {
        if (GameManager.instance.IsGamePaused())
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }

        // Mouse scroll wheel switching
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            SwitchWeapon(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            SwitchWeapon(-1);
        }

        // Number keys for weapon switching
        for (int i = 1; i <= Mathf.Min(weapons.Count, 9); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i - 1))
            {
                EquipWeapon(i - 1);
            }
        }
    }

    private void EquipWeapon(int index)
    {
        if (weapons == null || weapons.Count == 0)
        {
            Debug.LogWarning("Cannot equip weapon, weapon list is empty.");
            return;
        }

        if (index < 0 || index >= weapons.Count)
        {
            Debug.LogWarning("Weapon index out of range.");
            return;
        }
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
            currentWeapon.Unequip();
            currentWeapon.OnAmmoChanged.RemoveListener(UpdateAmmoCount);
        }

        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.Equip();

        currentWeapon.OnAmmoChanged.AddListener(UpdateAmmoCount);

        // Trigger weapon switch event
        OnWeaponSwitch.Invoke(currentWeapon);

        // Update ammo count immediately after equipping a new weapon
        UpdateAmmoCount();
    }

    private void SwitchWeapon()
    {
        if (GameManager.instance.IsGamePaused())
            return;
        if (weapons == null || weapons.Count == 0)
        {
            Debug.LogWarning("Cannot switch weapon, weapon list is empty.");
            return;
        }

        int nextWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        EquipWeapon(nextWeaponIndex);
    }
    //Overloaded function for using scrollwheel to swap weapons.
    private void SwitchWeapon(int direction)
    {
         if (weapons == null || weapons.Count == 0)
        {
            Debug.LogWarning("Cannot switch weapon, weapon list is empty.");
            return;
        }

        currentWeaponIndex += direction;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }
        else if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Count - 1;
        }

        EquipWeapon(currentWeaponIndex);   
    }

    private void UpdateAmmoCount()
    {
        if (ammoText != null && currentWeapon != null)
        {
            int currentAmmo = weapons[currentWeaponIndex].GetCurrentAmmo();
            ammoText.text = "Ammo: " + currentAmmo;
            Debug.Log("WeaponManager - UpdateAmmoCount called: " + currentAmmo);
        }
        else
        {
            ammoText.text = "Ammo: --/--";
            Debug.LogWarning("AmmoText or Weapon reference is not assigned!");
        }
    }

    public void HandleShooting()
    {
        if (GameManager.instance.IsGamePaused())
            return;

        if (currentWeapon != null && Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

}
