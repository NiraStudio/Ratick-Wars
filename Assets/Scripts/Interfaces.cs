﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
     void GetHit(float dmg);
     void Die();
}
public interface IAttackable
{
     void Attack();
    void AttackAnimation();
    void AttackAllower();

}
public interface IHealable
{
    void GetHeal(float amount);
}