using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IsoMetricHandler))]

public class Enemy : MainBehavior, IAttackable, IHitable, IHealable
{

    public EnemyData data;


    protected Rigidbody2D RG;
    protected GamePlayManager GPM;
    protected Collider2D detectedCharacter;
    protected Animator anim;
    protected float damage, attackSpeed, range, speed;
    protected float hp;


    bool attacking,Moving;
    bool free=true;
    float waitTime;
    Transform TargetTransform;
    Vector2 Direction;
    // Use this for initialization
    public virtual void Start () {
        RG = GetComponent<Rigidbody2D>();
        GPM = GamePlayManager.Instance;
        anim = GetComponent<Animator>();
        TargetTransform = Camera.main.transform;
        InitializeData();
    }

    // Update is called once per frame
    public virtual void Update () {

        if (GPM.gameState == GamePlayState.Finished)
        {
            StopWalk();
            return;
        }

        if (!free)
            return;



        if (attacking)
        {
            StopWalk();
            Moving = false;
        }
        else
        {
            RG.bodyType = RigidbodyType2D.Dynamic;
            Direction = TargetTransform.position - transform.position;
            Direction.Normalize();
            RG.velocity = Direction * speed;
            waitTime += Time.deltaTime;
            detectedCharacter = Physics2D.OverlapCircle(transform.position, range, CharacterLayer);
            Moving = true;
            if (detectedCharacter&&waitTime>=attackSpeed)
                AttackAnimation();
        }
        anim.SetBool("Move", Moving);

	}

    void InitializeData()
    {
        range = data.range;
        damage = data.damage;
        speed = data.speed;
        attackSpeed = data.attackSpeed;
        hp = data.hp;
    }

    void StopWalk()
    {
        if (RG.bodyType == RigidbodyType2D.Static)
            return;
        RG.velocity = Vector2.zero;
        RG.bodyType = RigidbodyType2D.Static;
    }


    public virtual void Attack()
    {
        if (detectedCharacter != null)
            detectedCharacter.SendMessage("GetHit", damage);
    }

    public void AttackAnimation()
    {
        attacking = true;
        anim.SetTrigger("Attack");
    }

    public void AttackAllower()
    {
        attacking = false;
        waitTime = 0;
    }

    public void GetHit(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
            Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void GetHeal(float amount)
    {
        throw new NotImplementedException();
    }







    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Reset()
    {
        gameObject.AddComponent<IsoMetricHandler>().Static = false;
        RG = GetComponent<Rigidbody2D>();
        RG.constraints = RigidbodyConstraints2D.FreezeRotation;
        RG.gravityScale = 0;
    }
}
