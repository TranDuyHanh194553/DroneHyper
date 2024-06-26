using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PretakenCollider : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TheBox theBox;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pretaken")
        {
            Debug.Log("s");
            rb.isKinematic = false;

            theBox.transform.SetParent(null);
            theBox.transform.localScale = theBox.transform.localScale * 2f;

            Invoke(nameof(ReturnSimulated), 1f);
            Destroy(gameObject);

        }
    }

    private void ReturnSimulated()
    {
        rb.simulated = true;
    }
}
