using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    public float currentSpeed;
    public float speed;
    public float jump;
    public float airSpeed;
    [HideInInspector] public Vector2 dashSpeed;
    public float dashStrength;
    public int maxDashes;
    public int dashes;

    [HideInInspector] public float dashTime;
    public float maxDashTime;
    [Header("Timer")]
    public float dashTimer;
    public float maxDashTimer;

    [Header("Rotation")]
    public float hoverRotationSpeed;
    public float walkRotationSpeed;

    [Header("Other")]
    public GameObject rayOrigin;
    public float rayCheckDistance;
    public float rayCheckSize;
    Rigidbody2D rb;
    public float dropOffTime = 0.2f;


    public LookatDirection LookAtScript;
    [HideInInspector] public Vector2 velocity;
    float gravity;

    bool applyingDashSpeed;
    bool dashing;
    public bool grounded = true;

    [Header("Sprites")]
    public Sprite[] spriteArray;

    [Header("")]
    public float initialWait;
    public bool debugging;
    bool ungrounding;
    bool justJumped = false;
    public bool showArrow;
    public bool started;
    PauseMenu PauseMenuScript;
    void Start()
    {
        PauseMenuScript = FindObjectOfType<PauseMenu>();
        applyingDashSpeed = false;
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartPlay());
    }

    void Update()
    {
        if (!PauseMenuScript.pause)
        {
            if (started)
            {
                switch (dashes)
                {
                    case 2:
                        GetComponent<SpriteRenderer>().sprite = spriteArray[2];
                        break;
                    case 1:
                        GetComponent<SpriteRenderer>().sprite = spriteArray[1];
                        break;
                    case 0:
                        GetComponent<SpriteRenderer>().sprite = spriteArray[0];
                        break;
                    default:
                        break;
                }
                if (applyingDashSpeed)
                {
                    dashTimer += Time.deltaTime;

                    if (dashTimer < maxDashTimer)
                    {
                        if (dashSpeed != Vector2.zero)
                        {
                            rb.velocity = dashSpeed;
                        }
                        else
                        {
                            Debug.Log("dash speed0");
                        }


                    }
                    else
                    {
                        dashTimer = 0.0f;
                        applyingDashSpeed = false;

                    }

                }

                Grounded();
                if (dashing)
                {
                    if (Input.GetAxis("Horizontal") >= 0)
                    {
                        rb.AddTorque(hoverRotationSpeed);
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        rb.AddTorque(-hoverRotationSpeed);

                    }
                    dashTime += Time.deltaTime;
                    currentSpeed = 0;
                    if (!Input.GetButton("Jump") || dashTime >= maxDashTime)
                    {
                        dashing = false;
                        MakeKinematic(false);
                        StartCoroutine(Dash());
                    }
                }
                else
                {
                    rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * currentSpeed*Time.deltaTime, 0));
                    if (Input.GetAxis("Horizontal") > 0 && grounded)
                    {
                        rb.AddTorque(-walkRotationSpeed * Time.deltaTime);
                    }
                    else if (Input.GetAxis("Horizontal") < 0 && grounded)
                    {
                        rb.AddTorque(walkRotationSpeed * Time.deltaTime);
                    }
                    else
                    {

                    }
                }

                if (Input.GetButtonDown("Jump"))
                {
                    if (grounded && !justJumped) // on the ground
                    {
                        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                        StartCoroutine(jumpReset());
                    }

                    else if (!grounded && dashes > 0 && !dashing) // not on the ground and still has dashes
                    {
                        dashing = true;
                        dashes--;
                        velocity = rb.velocity;
                        gravity = rb.gravityScale;
                        MakeKinematic(true);
                    }
                    else
                    {

                    }

                }
                // faster in air
                if (!grounded && currentSpeed != 0)
                {
                    currentSpeed = speed * airSpeed;
                }
                if (grounded)
                {
                    currentSpeed = speed;
                }

                if (debugging)
                {
                }
            }
        }

    }

    IEnumerator StartPlay()
    {
        yield return new WaitForSeconds(initialWait);
        started = true;
        GetComponent<Animator>().enabled = false;
    }
    void Grounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(rayOrigin.transform.position.x, rayOrigin.transform.position.y), Vector2.down, GetComponentInParent<BoxCollider2D>().bounds.extents.y + rayCheckDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(rayOrigin.transform.position.x + rayCheckSize, rayOrigin.transform.position.y), Vector2.down, GetComponentInParent<BoxCollider2D>().bounds.extents.y + rayCheckDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(rayOrigin.transform.position.x - rayCheckSize, rayOrigin.transform.position.y), Vector2.down, GetComponentInParent<BoxCollider2D>().bounds.extents.y + rayCheckDistance);
        
        if (hit.collider == null && hitLeft.collider == null && hitRight.collider == null)
        {
            if (!ungrounding && grounded)
            {
                StartCoroutine(UnGround());
            }

        }
        else
        {
            grounded = true;
            dashes = maxDashes;

        }
    }
    void MakeKinematic(bool boo)
    {
        if (boo)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

    }
    IEnumerator Dash()
    {
        

        if (LookAtScript.aim != Vector3.zero)
        {
            dashSpeed = Mathf.Clamp(dashTime, 0.5f, maxDashTime) * dashStrength * LookAtScript.aim;
            applyingDashSpeed = true;
        }
        dashTime = 0;
        yield return new WaitForSeconds(maxDashTimer);
        currentSpeed = speed;

    }

    public void ModifyDashes(int amount)
    {
        if (amount != 0)
        {
            dashes += amount;
            dashes = Mathf.Clamp(dashes, 0, maxDashes);

            if (amount >= 0) //added a dash
            {
                Debug.Log("dashes added: " + amount);
            }
            else //removed a dash
            {
                Debug.Log("dashes removed: " + amount);
            }
        }

        else // do nothing
        {

        }
    }

    public void Die()
    {
        //to implement
        Debug.Log("Should die");
    }

    IEnumerator UnGround()
    {
        ungrounding = true;
        yield return new WaitForSeconds(dropOffTime);
        grounded = false;
        ungrounding = false;
    }
    IEnumerator jumpReset()
    {
        justJumped = true;
        yield return new WaitForSeconds(dropOffTime);
        justJumped = false;
    }
}


