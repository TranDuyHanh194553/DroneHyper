using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{

    [SerializeField] private GameObject attackArea;

    private void Awake()
    {
        DeActiveAttack();
    }
    public override void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    protected void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    protected void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
}
