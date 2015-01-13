using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool jump = false;

    public bool playerControl = false;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public LayerMask whatIsGround;
    [HideInInspector]
    public FollowPath activePlatform;

    public bool atDoor = false;
    [HideInInspector]
    public GameObject doorAt;

    private Transform groundCheck;
    private RaycastHit2D groundHit;
    private bool grounded = false;
    private RaycastHit2D[] hits;
    private Animator anim;

    void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        groundHit = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
        grounded = groundHit;
        anim.SetBool("Ground", grounded);

        if (grounded)
        {
            if (groundHit.transform.tag == "MovingPlatform")
                activePlatform = groundHit.transform.GetComponent<FollowPath>();
        }
        else
            activePlatform = null;

        if (Input.GetButtonDown("Jump") && grounded && playerControl)
            jump = true;

        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

        HandleDoors();
    }

    void FixedUpdate()
    {
        if (playerControl)
        {
            float h = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(h));

            rigidbody2D.velocity = new Vector2(h * maxSpeed, rigidbody2D.velocity.y);

            if (activePlatform != null)
                rigidbody2D.velocity += activePlatform.velocity;

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
        rigidbody2D.velocity = Vector2.zero;
        anim.SetFloat("Speed", 0.0f);
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

