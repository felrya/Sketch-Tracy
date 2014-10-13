using UnityEngine;
using System.Collections;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject sketchPreFab;
    public GameObject tracyPreFab;
    public GameObject teamPreFab;

    public GameObject sketchObject;
    public GameObject tracyObject;
    public GameObject teamObject;
    public GameObject cameraObject;

    private CharacterController sketch;
    private CharacterController tracy;
    private CameraFollow camera;
    private bool sketchActiveStatus;
    private bool tracyActiveStatus;

    private bool teamedUp = false;
    public float teamDistance = 0.75f;

    // Use this for initialization
    void Start()
    {
        sketch = sketchObject.GetComponent<CharacterController>();
        tracy = tracyObject.GetComponent<CharacterController>();
        camera = cameraObject.GetComponent<CameraFollow>();
    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchCharacter"))
        {
            sketchActiveStatus = sketch.playerControl;
            tracyActiveStatus = tracy.playerControl;

            if (tracyActiveStatus)
            {
                tracy.Deactivate();
                sketch.Activate();
                changeCameraTarget(sketchObject);
            }
            else if (sketchActiveStatus)
            {
                sketch.Deactivate();
                tracy.Activate();
                changeCameraTarget(tracyObject);
            }
        }

        if (Input.GetButtonDown("TeamUp"))
        {
            if (!teamedUp)
            {
                if (canTeamUp())
                    teamUp();
            }
            else
            {
                unTeam();
            }
        }
    }

    private bool canTeamUp()
    {
        if (Mathf.Abs(Vector3.Distance(sketchObject.transform.position, tracyObject.transform.position)) < teamDistance)
            return true;
        else
            return false;
    }

    void teamUp()
    {
        teamedUp = true;

        Vector3 spawnLocation = sketchObject.transform.position;
        Quaternion rotation = sketchObject.transform.rotation;

        sketch.playerControl = false;
        tracy.playerControl = false;

        Destroy(sketchObject);
        Destroy(tracyObject);

        teamObject = (GameObject)Instantiate(teamPreFab, spawnLocation, rotation);
        teamObject.name = "Team";
        teamObject.GetComponent<CharacterController>().playerControl = true;

        changeCameraTarget(teamObject);
    }

    void unTeam()
    {
        Vector3 spawnLocation = teamObject.transform.position;
        Quaternion rotation = teamObject.transform.rotation;

        teamObject.GetComponent<CharacterController>().playerControl = false;
        Destroy(teamObject);

        sketchObject = (GameObject)Instantiate(sketchPreFab, spawnLocation - new Vector3(0.5f, 0, 0), rotation);
        sketchObject.name = "Sketch";
        sketch = sketchObject.GetComponent<CharacterController>();
        sketch.playerControl = true;

        tracyObject = (GameObject)Instantiate(tracyPreFab, spawnLocation + new Vector3(0.5f, 0, 0), rotation);
        tracyObject.name = "Tracy";
        tracy = tracyObject.GetComponent<CharacterController>();

        teamedUp = false;

        changeCameraTarget(sketchObject);
    }

    void changeCameraTarget(GameObject newTarget)
    {
        camera.SetTarget(newTarget.transform);
    }
}

