using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployEnemy : MonoBehaviour,IHitable {

    [SerializeField]
    CharacterData data;
    [SerializeField]
    bool right;


    protected GamePlayInput GPI;
    protected Rigidbody2D RG;
    protected GamePlayManager GPM;
    protected GameManager GM;
    protected Collider2D detectedEnemy;
    protected Animator anim;
    protected float damage, attackSpeed, range, speed;
    protected int hp;

    Transform enemy;
    Enemy[] Enemies;

    Vector2 direction;
    public virtual void Start()
    {
        RG = GetComponent<Rigidbody2D>();
        GPI = GamePlayInput.Instance;
        GPM = GamePlayManager.Instance;
        GM = GameManager.Instance;
        anim = GetComponent<Animator>();
        InitializeData();
        FindNearestEnemy();
    }

    public virtual void Update()
    {
        if (enemy == null)
            if (!FindNearestEnemy())
            {
                RG.velocity = Vector2.zero;
                return;
            }

        direction = enemy.position-transform.position;
        direction.Normalize();
        RG.velocity = direction * 5;

        if(Vector2.Distance(transform.position,enemy.position)<=range)
        {
            Attack();
            Die();
        }
    }

    bool FindNearestEnemy()
    {
        Enemies = GameObject.FindObjectsOfType<Enemy>();
        if (Enemies.Length < 1)
            return false;

        enemy = Enemies[0].transform;
        float dis= Vector2.Distance(transform.position, enemy.position);
        float t;
        foreach (var item in Enemies)
        {
            t = Vector2.Distance(transform.position, item.transform.position);
            if (t < dis)
            {
                enemy = item.transform;
                dis = t;
            }
        }

        return true;
    }
    void InitializeData()
    {
        range = data.range;
        damage = data.damage;
        speed = data.speed;
        attackSpeed = data.attackSpeed;
        hp = data.hp;
        //Upgrade The State();
    }

    void Flip()
    {

        Vector3 aa = transform.localScale;
        aa.x *= -1;
        transform.localScale = aa;
        right = !right;
    }
    void StopWalk()
    {

        if (RG.bodyType == RigidbodyType2D.Static)
            return;
        RG.velocity = Vector2.zero;
        RG.bodyType = RigidbodyType2D.Static;
    }

    public void GetHeal(float amount)
    {
        throw new NotImplementedException();
    }

    public void GetHit(float dmg)
    {

        hp -= (int)dmg;
        if (hp <= 0)
            Die();
    }

    public virtual void Attack()
    {
        enemy.gameObject.SendMessage("GetHit", damage);
    }



    void Reset()
    {
        gameObject.AddComponent<IsoMetricHandler>().Static = false;
        RG = GetComponent<Rigidbody2D>();
        RG.constraints = RigidbodyConstraints2D.FreezeRotation;
        RG.gravityScale = 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
