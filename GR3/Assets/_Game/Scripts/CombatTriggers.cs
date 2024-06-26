using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTriggers : GameUnit
{
    void Start()
    {
        OnInit();
    }
    public void OnInit()
    {
    }

    public void OnDespawn()
    {
        //Destroy(gameObject);
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            OnDespawn();
        }
    }
}
