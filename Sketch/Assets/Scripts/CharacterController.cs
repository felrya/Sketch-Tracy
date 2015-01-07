using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool jump = false;

    public bool playerControl = false;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public float springForce = 750f;
    public LayerMask whatIsGround;

    public bool atDoor = false;
    public GameObject doorAt;

    private Transform groundCheck;
    private bool grounded = false;
    private Animator anim;

    void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
        anim.SetBool("Ground", grounded);

        if (Input.GetButtonDown("Jump") && grounded && playerControl)
            jump = true;

        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
    }

    void FixedUpdate()
    {
        if (playerControl)
        {
            float h = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(h));

            rigidbody2D.velocity = new Vector2(h * maxSpeed, rigidbody2D.velocity.y);

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                rigidbody2D.AddForce(new Vector2(0f, jumpForce));

                jump = false;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Activate()
    {
        playerControl = true;
    }

    public void Deactivate()
    {
        playerControl = false;
    }

    private void HandleDoors()
    {
        Door theDoor;
        if (atDoor && Input.GetButtonDown("OpenDoor"))
        {
            if (doorAt != null)
            {
                theDoor = doorAt.GetComponent<Door>();
                if (!theDoor.isOpen)
                {
                    theDoor.OpenDoor(gameObject);
                }
                else
                {
                    playerControl = false;
                    theDoor.CloseDoor();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "SpringSingle" && col.relativeVelocity.y < 0)
        {
            Debug.Log(col.relativeVelocity.x + ", " + col.relativeVelocity.y);
            rigidbody2D.AddForce(new Vector2(0, springForce));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            atDoor = true;
            doorAt = col.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            atDoor = false;
            doorAt = null;
        }
    }
}

