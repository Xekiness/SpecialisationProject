using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    private Sniper sniper; // Reference to the Sniper weapon script
    [SerializeField] private TMP_Text ammoText;

    private void Start()
    {
        // Find the Sniper script in the scene
        sniper = FindObjectOfType<Sniper>();

        if (sniper == null)
        {
            //Debug.LogError("Sniper script not assigned to AmmoCounter!");
            Debug.LogError("Sniper script not found in the scene!");
            return;
        }
        else
        {
            // Update the ammo count UI initially
            UpdateAmmoCount();
        }
    }

    public void UpdateAmmoCount()
    {
        if (ammoText != null && sniper != null)
        {
            ammoText.text = "Ammo: " + sniper.GetCurrentAmmo() + "/" + sniper.GetMaxAmmo();
        }
        else
        {
            Debug.LogWarning("AmmoText or Sniper reference is not assigned!");
        }
    }

    //public void UpdateAmmoCount(int currentAmmo, int maxAmmo)
    //{
    //    if (ammoText != null)
    //    {
    //        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    //    }
    //}
}
