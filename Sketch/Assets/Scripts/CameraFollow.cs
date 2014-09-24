/*using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Vector3 offset = new Vector3(0f, 1.5f, 0f);

	public Vector2 cameraRangeX = new Vector2(2.0f, 10.0f);
	public Vector2 cameraRangeY = new Vector2(0.0f, 100.0f);

	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			Vector3 offset = new Vector3(0f, 1.5f, 0f);
			Vector3 point = camera.WorldToViewportPoint(target.position + offset);
			Vector3 delta = target.position + offset - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + offset + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}*/

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	
	public Transform target;
	private float trackSpeed = 10;
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
