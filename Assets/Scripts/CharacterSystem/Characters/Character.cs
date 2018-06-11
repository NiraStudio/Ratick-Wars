using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IsoMetricHandler))]

public class Character : MainBehavior, IAttackable, IHitable, IHealable
{
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

    bool gathering,moving,died;
   public bool attacking;
    Vector2 tt;
    float waitTime;
    public virtual void Awake()
    {

    }
    public virtual void Start()
    {
        RG = GetComponent<Rigidbody2D>();
        GPI = GamePlayInput.Instance;
        GPM = GamePlayManager.Instance;
        GM = GameManager.Instance;
        anim = GetComponent<Animator>();
        InitializeData();
    }
    public virtual void Update()
    {
        if (GPM.gameState == GamePlayState.Finished)
        {
            StopWalk();
            return;
        }

        waitTime += Time.deltaTime;
        if (gathering)
        {

            moving = false;
        }
        else
        {
            if (attacking)
            {
                #region Flip if EnemyNearBy
                StopWalk();
                if (detectedEnemy != null)
                {
                    tt = (detectedEnemy.transform.position - transform.position);
                    if (tt.x > 0 && !right)
                    {
                        Flip();
                    }
                    else if (tt.x < 0 && right)
                    {
                        Flip();

                    }
                }

                #endregion
            }
            else
            {
                #region Move

                if (GPI.move)
                {
                    RG.bodyType = RigidbodyType2D.Dynamic;
                    RG.velocity = GPI.direction * speed;
                    tt = GPI.direction;

                    if (tt.x > 0 && !right)
                    {
                        Flip();
                    }
                    else if (tt.x < 0 && right)
                    {
                        Flip();

                    }

                }
                else
                    StopWalk();

                #endregion


                #region AttackDetection
                if (detectedEnemy == null)
                    detectedEnemy = Physics2D.OverlapCircle(transform.position, range, EnemyLayer);

                if (waitTime > attackSpeed && detectedEnemy != null)
                    AttackAnimation();
                #endregion
            }
            moving = GPI.move;

        }
        #region Animation
        if (anim)
        {
            anim.SetBool("Move", moving);
        }

        #endregion


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
    public void StartGatthering()
    {
        Vector2 middle = Camera.main.transform.position;
        middle -= (Vector2)CameraController.Instance.offSet;
        if (Vector2.Distance(transform.position, middle) > 1f)
            StartCoroutine(Gather(middle));
    }

    public IEnumerator Gather(Vector2 middle)
    {

        Vector2 direction;
        
        gathering = true;
        RG.bodyType = RigidbodyType2D.Dynamic;
        for (float i = 0; i < 0.5f; i+=Time.deltaTime)
        {
            if (died)
                break;
            if (Vector2.Distance(transform.position, middle) < .1f)
                break;
            direction = middle - (Vector2)transform.position;
            direction.Normalize();
            if (RG.bodyType != RigidbodyType2D.Static)
                RG.velocity = direction * speed * 2;
            yield return null;
        }
            
        StopWalk();
        gathering = false;
    }

    void StopWalk()
    {

        if (died)
            return;
        if (RG.bodyType == RigidbodyType2D.Static)
            return;
        RG.velocity = Vector2.zero;
        RG.bodyType = RigidbodyType2D.Static;
    }
    public virtual void Attack()
    {
        if (detectedEnemy != null)
            detectedEnemy.SendMessage("GetHit", damage);

    }

    public void AttackAllower()
    {
        attacking = false;
        waitTime = 0;
    }

    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
        attacking = true;
    }

    public void Die()
    {
        died = true;
        
        Destroy(gameObject);
    }

    public void GetHeal(float amount)
    {
        throw new NotImplementedException();
    }

    public void GetHit(float dmg)
    {

        hp -=(int) dmg;
        if (hp <= 0)
            Die();
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
public enum CharacterMoveState
{
    InArea,Far
}
