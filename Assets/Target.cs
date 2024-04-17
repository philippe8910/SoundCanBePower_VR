using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour , IDamageable
{
    public int DamageCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action OnDestroyed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void TakeDamage(int amount)
    {
        DamageCount -= amount;

        if(DamageCount <= 0)
        {
            OnDestroyed();
        }
    }
}
