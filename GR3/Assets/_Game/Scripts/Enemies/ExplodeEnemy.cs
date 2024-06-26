using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : Enemy
{
    public GameObject hit_VFX;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Player")
        {
            ChangeAnim("attack");

            //Player takes damage
            Character c = collision.GetComponent<Character>();
            c.OnHit(30f);
            c.moveSpeed = c.moveSpeed * 0.8f;

            // This takes damage
            OnHit(50f);

            Instantiate(hit_VFX, transform.position, transform.rotation);
        }
    }
}
