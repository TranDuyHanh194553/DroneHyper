using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBox : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject death_VFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ObjTaker")
        {
            Player player = FindObjectOfType<Player>();
            player.boxNumber++;
            player.jumpForce = player.jumpForce * 0.6f;

            rb.isKinematic = true;
            rb.simulated = false;

            this.transform.SetParent(collision.transform);
            transform.position = collision.transform.position;
        }

        if (collision.tag == "Bullet" || collision.tag == "DeathZone")
        {
            Instantiate(death_VFX, transform.position, transform.rotation);
            Invoke(nameof(GameOver), 1f);
            gameObject.SetActive(false);
        }
    }

    private void GameOver()
    {
        UIManager.instance.OpenLose();
    }
}
