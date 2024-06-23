using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons;
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    public UnityEvent<Weapon> OnWeaponSwitch;

    private void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
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
        }

        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.Equip();

        // Trigger weapon switch event
        OnWeaponSwitch.Invoke(currentWeapon);
    }

    private void SwitchWeapon()
    {
        if (weapons == null || weapons.Count == 0)
        {
            Debug.LogWarning("Cannot switch weapon, weapon list is empty.");
            return;
        }

        int nextWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        EquipWeapon(nextWeaponIndex);
    }

    public void HandleShooting()
    {
        if (currentWeapon != null && Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
