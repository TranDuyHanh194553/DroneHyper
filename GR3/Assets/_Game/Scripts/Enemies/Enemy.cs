using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject death_VFX;

    private IState currentState;

    private Character target;
    public Character Target => target;

    private bool isRight = true;
    private void Update()
    {
        if (IsDead)
        {
            return;
        }

        if (currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();

        ChangeState(new IdleState());
        
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        //Destroy(healthBar.gameObject);
        //Destroy(gameObject);

        if (transform.parent != null)
        {
            Instantiate(death_VFX, transform.position, transform.rotation);
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject); // In case there's no parent, destroy the enemy itself
        }
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath(); 
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null) 
        { 
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null )
        {
            currentState.OnEnter(this);
        }
    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else
        if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public virtual void Attack()
    {
        
    }

    public bool IsTargetInRange()
    {
        if (Target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        return false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }

        if (collision.tag == "DeathZone")
        {
            ChangeAnim("die");
            OnDeath();
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

}
