﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Game Data/Enemy Data" )]
public class EnemyData : ScriptableObject {

    public string enemyName;
    public float damage;
    public float hp;
    public float attackSpeed;
    public float speed;
    public float range;
    public int ratick;
}
