using UnityEngine;
using System.Collections;


public class CharacterInputHandler : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    private bool _facingRight = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;

        if (_controller.isGrounded)
            _velocity.y = 0;

        // get horizontal movement
        normalizedHorizontalSpeed = Input.GetAxis("Horizontal");

        // handle sprite flipping
        if (normalizedHorizontalSpeed > 0 && !_facingRight)
            Flip();
        else if (normalizedHorizontalSpeed < 0 && _facingRight)
            Flip();

        _animator.SetBool("Ground", _controller.isGrounded);

        // we can only jump whilst grounded
        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.SetBool("Ground", false);
        }

        // apply horizontal speed smoothing it
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        _animator.SetFloat("Speed", Mathf.Abs(_controller.velocity.x));

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        _animator.SetFloat("vSpeed", _controller.velocity.y);

        _controller.move(_velocity * Time.deltaTime);
    }

    void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
