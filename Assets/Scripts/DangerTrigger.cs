using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (!collision.gameObject.GetComponent<PlayerController>().respawning)
            {
                StartCoroutine(collision.gameObject.GetComponent<PlayerController>().Die());
            }
        }
    
    }

}
