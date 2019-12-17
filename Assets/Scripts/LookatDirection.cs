using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatDirection : MonoBehaviour
{
    public float deadZone;
    Vector3 directionVector;
    public Vector3 aim;
    PlayerController player;
    bool waited;
    PauseMenu PauseMenuScript;
    private void Start()
    {
        PauseMenuScript = FindObjectOfType<PauseMenu>();

        directionVector = Vector3.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        aim = Vector3.zero;
        player = GetComponentInParent<PlayerController>();
        StartCoroutine(WaitStart());
    }
    void Update()
    {
        if (!PauseMenuScript.pause)
        {
            if (player.started && waited)
            {

                directionVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                if (directionVector.magnitude >= deadZone && new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude >= deadZone && !player.grounded)
                {
                    aim = directionVector;

                }
                else
                {
                    directionVector = Vector3.zero;
                    aim = Vector3.zero;

                }
                if (!player.grounded)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;

                }

                float rot_z = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
            }
        }
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(6);
        waited = true;
    }
}
