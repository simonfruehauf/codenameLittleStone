using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashModifier : MonoBehaviour
{
    public int dashModifier;
    bool disabled = false;
    public SpriteRenderer sprite;
    private float disableTime;
    void Start()
    {
        disableTime = GameObject.Find("Player").GetComponent<PlayerController>().initialWait;
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!disabled)
        {
            Debug.Log(disabled);
            if (collision.gameObject.GetComponent<PlayerController>() != null) //see if player
            {
                Debug.Log("Collision");
                collision.gameObject.GetComponent<PlayerController>().ModifyDashes(dashModifier);

                if (disableTime == 0)
                {

                    enabled = false;
                    sprite.enabled = false;
                }
                else
                {
                    StartCoroutine(Disable());
                }

            }
            else
            {
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    IEnumerator Disable()
    {
        disabled = true;
        sprite.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(disableTime);
        disabled = false;
        sprite.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

    }
}
