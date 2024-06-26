using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{

    [SerializeField] private Transform throwPoint;

    private bool isAttack = false;

    private void Awake()
    {
        isAttack = false;
    }
    public override void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        //Instantiate(bulletPrefab, throwPoint.position, throwPoint.rotation);
        Bullet b = SimplePool.Spawn<Bullet>(PoolType.Bullet_1, throwPoint.position, throwPoint.rotation);
        b.OnInit();
    }

    private void ResetAttack()
    {
        ChangeAnim("run");
        isAttack = false;
    }


}
