//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class AmmoCounter : MonoBehaviour
//{
//    //private Sniper sniper; // Reference to the Sniper weapon script
//    //private Shotgun shotgun;

//    private Weapon currentWeapon;

//    [SerializeField] private TMP_Text ammoText;

//    //TODO: Make ammo counter modular

//    private void Start()
//    {
//        // Find the Sniper script in the scene
//        //sniper = FindObjectOfType<Sniper>();
//        //shotgun = FindObjectOfType<Shotgun>();

//        currentWeapon = FindCurrentWeapon();

//        if (currentWeapon == null)
//        {
//            Debug.LogError("Weapon script not found in the scene!");
//            return;
//        }




//        //if (shotgun == null)
//        //{
//        //    Debug.LogError("shotgun script not found in the scene!");
//        //    return;
//        //}

//        //if (sniper == null)
//        //{
//        //    Debug.LogError("Sniper script not found in the scene!");
//        //    return;
//        //}

//        // Subscribe to the ammo change event
//        //shotgun.OnAmmoChanged.AddListener(UpdateAmmoCount);
//        //sniper.OnAmmoChanged.AddListener(UpdateAmmoCount);
//        currentWeapon.OnAmmoChanged.AddListener(UpdateAmmoCount);

//        // Update the ammo count UI initially
//        UpdateAmmoCount();
//    }

//    private Weapon FindCurrentWeapon()
//    {
//        // Find the current weapon script in the scene
//        // This method can be improved to dynamically get the currently equipped weapon
//        return FindObjectOfType<WeaponManager>().GetCurrentWeapon();
//    }

//    public void UpdateAmmoCount()
//    {
//        if (ammoText != null && currentWeapon != null)
//        {
//            ammoText.text = "Ammo: " + currentWeapon.GetCurrentAmmo() + "/" + currentWeapon.GetReserveAmmo();
//        }
//        else
//        {
//            Debug.LogWarning("AmmoText or Weapon reference is not assigned!");
//        }
//    }

//    //public void UpdateAmmoCount()
//    //{
//    //    if (ammoText != null && sniper != null)
//    //    {
//    //        ammoText.text = "Ammo: " + sniper.GetCurrentAmmo() + "/" + sniper.GetReserveAmmo();
//    //        //ammoText.text = "Ammo: " + shotgun.GetCurrentAmmo() + "/" + shotgun.GetReserveAmmo();
//    //    }
//    //    else
//    //    {
//    //        Debug.LogWarning("AmmoText or Sniper reference is not assigned!");
//    //    }
//    //}
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    private Weapon currentWeapon;
    private WeaponManager weaponManager;

    [SerializeField] private TMP_Text ammoText;

    private void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();

        if (weaponManager == null)
        {
            Debug.LogError("WeaponManager not found in the scene!");
            return;
        }

        // Find the initial current weapon
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon != null)
        {
            // Subscribe to the ammo change event
            currentWeapon.OnAmmoChanged.AddListener(UpdateAmmoCount);
        }

        // Subscribe to weapon switch event
        weaponManager.OnWeaponSwitch.AddListener(UpdateCurrentWeapon);

        // Update the ammo count UI initially
        UpdateAmmoCount();
    }

    private void UpdateCurrentWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            // Unsubscribe from the old weapon's event
            currentWeapon.OnAmmoChanged.RemoveListener(UpdateAmmoCount);
        }

        currentWeapon = newWeapon;

        if (currentWeapon != null)
        {
            // Subscribe to the new weapon's event
            currentWeapon.OnAmmoChanged.AddListener(UpdateAmmoCount);
        }

        // Update the ammo count UI
        UpdateAmmoCount();
    }

    public void UpdateAmmoCount()
    {
        if (ammoText != null && currentWeapon != null)
        {
            int currentAmmo = currentWeapon.GetCurrentAmmo();
            int reserveAmmo = currentWeapon.GetReserveAmmo();
            ammoText.text = "Ammo: " + currentAmmo + "/" + reserveAmmo;
            Debug.Log("UpdateAmmoCount called: " + currentAmmo + " / " + reserveAmmo);
        }
        else
        {
            ammoText.text = "Ammo: --/--";
            Debug.LogWarning("AmmoText or Weapon reference is not assigned!");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            UpdateAmmoCount();
            Debug.Log("AmmoCount called");
        }
    }
}
