using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform firePoint;

    public override void Attack(Vector2 targetPosition)
    {
        if (Time.time >= nextAttackTime)
        {
            // Perform attack
            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
            GameObject bullet = Instantiate(weaponData.bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * weaponData.bulletSpeed;

            Destroy(bullet, 2f);

            nextAttackTime = Time.time + 1f / weaponData.fireRate;
        }
    }

    public override int GetCurrentAmmo()
    {
        return CurrentAmmo;
    }

    public override int GetReserveAmmo()
    {
        return ReserveAmmo;
    }
}
