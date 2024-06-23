using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    void OnDamaged(int damage);
    void Destroy();
    int CheckHealth();
}
