using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float dampTime = 0.15f;

	public float camMinX = 0;
	public float camMaxX = 10;
	public float camMinY = 0;
	public float camMaxY = 10;

	private Vector3 velocity = Vector3.zero;
	
	// Set target
	public void SetTarget(Transform t)
    {
		target = t;
	}
	
	// Track target
	void LateUpdate()
    {
		if (target)
        {
			Vector3 destination = transform.position;
			destination.x = Mathf.Clamp(target.position.x, camMinX, camMaxX);
			destination.y = Mathf.Clamp(target.position.y, camMinY, camMaxY);

			//transform.position = Vector3.MoveTowards (transform.position, v, trackSpeed * Time.deltaTime);
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}
}