using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public override void Attack(Vector2 targetPosition)
    {
        if (Time.time >= nextAttackTime)
        {
            // Perform attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                // Damage the enemy
                Debug.Log("Hit " + enemy.name);
            }

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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
