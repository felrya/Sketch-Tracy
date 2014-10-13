using UnityEngine;
using System.Collections;

public class SketchController : MonoBehaviour
{
	public float maxSpeed = 4;
	bool facingRight = true;

	Animator anim;

	bool grounded = false;
    bool onPlatform = false;
	public Transform groundCheck;
	float groundRadius = 0.9f;
	public LayerMask whatIsGround;
    public LayerMask platforms;
	public float jumpForce = 500f;
	public float springForce = 750f;

    public GameObject StandingOn;
    public Vector2 PlatformVelocity;

    private Vector2 platformPositionPrevious;
    private Vector2 platformPositionCurrent;

    /*private Vector3
        activeGlobalPlatformPoint,
        activeLocalPlatformPoint;*/

    private RaycastHit2D rayHit;

    // Moving platform support
    private Transform activePlatform;
    private Vector3 activeLocalPlatformPoint;
    private Vector3 activeGlobalPlatformPoint;
    private Vector3 lastPlatformVelocity;
 
    // If you want to support moving platform rotation as well:
    private Quaternion activeLocalPlatformRotation;
    private Quaternion activeGlobalPlatformRotation;

	public bool playerControl;
	
	private float move = 0.0f;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
		playerControl = true;
	}

	void FixedUpdate()
	{
		//Detect ground with circle
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		//Detect ground with linecast
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);

        /*rayHit = Physics2D.Linecast(transform.position, groundCheck.position, platforms);
        if (rayHit)
        {
            activePlatform = rayHit.transform;
            //StandingOn = rayHit.transform.gameObject;
            Debug.Log(PlatformVelocity);
        }
        else
        {
            StandingOn = null;
        }*/

		anim.SetBool ("Ground", grounded);

		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		//if (!grounded) return;

		if (playerControl && grounded && Input.GetButtonDown("Jump"))
		{
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}

        HandlePlatforms();

        if (playerControl)
        {
            move = Input.GetAxis("Horizontal");
        }
        else
        {
            move = 0.0f;
        }

        anim.SetFloat("Speed", Mathf.Abs(move));

        rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();

        if (activePlatform != null)
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);

            // If you want to support moving platform rotation as well:
            activeGlobalPlatformRotation = transform.rotation;
            activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;
        }

        /*if (StandingOn != null)
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);
        }*/
	}

	void Update()
	{

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

    void HandlePlatforms()
    {
        if (activePlatform != null)
        {
            var newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
            var moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
            if (moveDistance != Vector3.zero)
                transform.position += moveDistance;
            lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;

            // If you want to support moving platform rotation as well:
            var newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;
            var rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);

            // Prevent rotation of the local up vector
            rotationDiff = Quaternion.FromToRotation(rotationDiff * transform.up, transform.up) * rotationDiff;

            transform.rotation = rotationDiff * transform.rotation;
        }
        else
        {
            lastPlatformVelocity = Vector3.zero;
        }

        activePlatform = null; 
        /*if (StandingOn != null)
        {
            if (platformPositionPrevious == Vector2.zero)
                platformPositionPrevious = StandingOn.transform.position;

            platformPositionCurrent = StandingOn.transform.position;

            PlatformVelocity = (platformPositionCurrent - platformPositionPrevious) / Time.deltaTime;

            platformPositionPrevious = platformPositionCurrent;
        }*/
        /*if (StandingOn != null)
        {
            var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(activeLocalPlatformPoint);
            var moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;

            if (moveDistance != Vector3.zero)
                transform.Translate(moveDistance, Space.World);

            PlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
        }
        else
            PlatformVelocity = Vector3.zero;

        StandingOn = null;*/
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "SpringSingle" && col.relativeVelocity.y < 0)
		{
			Debug.Log (col.relativeVelocity.x + ", " + col.relativeVelocity.y);
			rigidbody2D.AddForce(new Vector2(0, springForce));
		}

        if (grounded && col.gameObject.tag == "Platform")
        {
            activePlatform = col.transform;
        }
	}
}
