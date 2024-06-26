using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] float damage = 30;
    [SerializeField] float speed = 5f;
    [SerializeField] float existTime = 4f;
    public GameObject hit_VFX, hitGround_VFX;
    public Rigidbody2D rb;
    
    //Combat
    [SerializeField] private float slowAmount = 1f;

    //private Vector2 direction;
    public void OnInit()
    {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), existTime);
    }

    public void OnDespawn()
    {
        //Destroy(gameObject); 
        SimplePool.Despawn(this);
    }

    //public void Shoot(Vector2 direction)
    //{
    //    this.direction = direction;
    //    rb.velocity = direction;
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Player") 
        {
            Character target = collision.GetComponent<Character>();
            AudioManager.Ins.PlaySFX(AudioManager.Ins.bulletHit);

            //Combat Impact
            target.OnHit(damage);
            target.moveSpeed = target.moveSpeed * slowAmount;

            Instantiate(hit_VFX, transform.position, transform.rotation);
            OnDespawn();
        }

        if (collision.tag == "MovingPlatform" || collision.tag == "Ground")
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.bulletHit);
            Instantiate(hitGround_VFX, transform.position, transform.rotation);
            OnDespawn();
        }

        if (collision.tag == "TheBox")
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.bulletHit);
            //Destroy(collision.gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    life--;
    //    if (life < 0)
    //    {
    //        OnDespawn();
    //        return;
    //    }

    //    var firstContact = collision.contacts[0];
    //    Vector2 newVelocity = Vector2.Reflect(direction.normalized, firstContact.normal);
    //    Shoot(newVelocity.normalized);
    //}
}
