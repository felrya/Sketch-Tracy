﻿using UnityEngine;
using System.Collections;

public class TracyController : MonoBehaviour
{
	public float maxSpeed = 4;
	bool facingRight = true;
	
	Animator anim;
	
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.9f;
	public LayerMask whatIsGround;
	public float jumpForce = 500f;
	public float springForce = 750f;

	public bool playerControl;

	private float move = 0.0f; 

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
		playerControl = false;
	}

	void FixedUpdate()
	{
		//Detect ground with circle
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		
		//Detect ground with linecast
		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
		
		anim.SetBool ("Ground", grounded);
		
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		
		//if (!grounded) return;

		if (playerControl)
		{
			move = Input.GetAxis ("Horizontal");
		}
		else
		{
			move = 0.0f;
		}
		
		anim.SetFloat("Speed", Mathf.Abs(move));
		
		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);

		if (playerControl && grounded && Input.GetButtonDown("Jump"))
		{
			anim.SetBool ("Ground", false);
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}
		
		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();
	}
	
	// Update is called once per frame
	void Update ()
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
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "SpringSingle" && col.relativeVelocity.y < 0)
		{
			Debug.Log (col.relativeVelocity.x + ", " + col.relativeVelocity.y);
			rigidbody2D.AddForce(new Vector2(0, springForce));
		}
	}
}
