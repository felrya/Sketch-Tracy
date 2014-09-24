using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject sketchObject;
	public GameObject tracyObject;
	public GameObject cameraObject;

	private SketchController sketch;
	private TracyController tracy;
	private CameraFollow camera;
	private bool sketchActiveStatus;
	private bool tracyActiveStatus;
	

	// Use this for initialization
	void Start()
	{
		sketch = sketchObject.GetComponent<SketchController>();
		tracy = tracyObject.GetComponent<TracyController>();
		camera = cameraObject.GetComponent<CameraFollow>();
	}

	void FixedUpdate()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("SwitchCharacter"))
		{
			sketchActiveStatus = sketch.playerControl;
			tracyActiveStatus = tracy.playerControl;

			if(tracyActiveStatus)
			{
				tracy.Deactivate();
				sketch.Activate();
				changeCameraTarget(sketchObject);
			}
			else if(sketchActiveStatus)
			{
				sketch.Deactivate();
				tracy.Activate();
				changeCameraTarget(tracyObject);
			}
		}
	}

	void changeCameraTarget(GameObject newTarget)
	{
		camera.SetTarget(newTarget.transform);
	}
}
