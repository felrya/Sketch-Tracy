using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Ball Controller")]
public class BallController : MonoBehaviour
{
    public float speed = 500;
    public float torque = 5;
    public float jumpForce = 2000;

    private bool jump = false;

	// Use this for initialization
	void Start ()
    {
	
	}

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float move = Input.GetAxis("Horizontal");
        float force = move * speed;
        //rigidbody2D.AddTorque(-torque * move * Time.deltaTime);
        rigidbody2D.AddForce(transform.right * force * Time.deltaTime);

        if (jump)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
	}
}
