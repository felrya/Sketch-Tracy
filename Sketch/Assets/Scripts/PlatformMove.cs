using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour
{
	public float directionSwitchTime = 2;
	private float timer;
	public float moveSpeed_X = 3;
	public float moveSpeed_Y = 0;

	// Use this for initialization
	void Start ()
	{
		timer = directionSwitchTime;
		rigidbody2D.velocity = new Vector2 (moveSpeed_X, moveSpeed_Y);
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer -= Time.deltaTime;

		if (timer < 0)
		{
			rigidbody2D.velocity *= -1;
			timer = directionSwitchTime;
		}

	}
}
