using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Shoot(Vector2 targetPosition);

    public virtual void Reload()
    {
    }
}
