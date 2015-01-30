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

    private PlayerController sketch;
    private PlayerController tracy;
    private CameraFollow cam;
    private bool sketchActiveStatus;
    private bool tracyActiveStatus;

    private bool teamedUp = false;
    public float teamDistance = 0.75f;

    // Use this for initialization
    void Start()
    {
        sketch = sketchObject.GetComponent<PlayerController>();
        tracy = tracyObject.GetComponent<PlayerController>();
        cam = cameraObject.GetComponent<CameraFollow>();
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
                ChangeCameraTarget(sketchObject);
            }
            else if (sketchActiveStatus)
            {
                sketch.Deactivate();
                tracy.Activate();
                ChangeCameraTarget(tracyObject);
            }
        }

        if (Input.GetButtonDown("TeamUp"))
        {
            if (!teamedUp)
            {
                if (CanTeamUp())
                    TeamUp();
            }
            else
            {
                UnTeam();
            }
        }
    }

    private bool CanTeamUp()
    {
        if (Mathf.Abs(Vector3.Distance(sketchObject.transform.position, tracyObject.transform.position)) < teamDistance)
            return true;
        else
            return false;
    }

    void TeamUp()
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
        teamObject.GetComponent<PlayerController>().playerControl = true;

        ChangeCameraTarget(teamObject);
    }

    void UnTeam()
    {
        Vector3 spawnLocation = teamObject.transform.position;
        Quaternion rotation = teamObject.transform.rotation;

        teamObject.GetComponent<PlayerController>().playerControl = false;
        Destroy(teamObject);

        sketchObject = (GameObject)Instantiate(sketchPreFab, spawnLocation - new Vector3(0.5f, 0, 0), rotation);
        sketchObject.name = "Sketch";
        sketch = sketchObject.GetComponent<PlayerController>();
        sketch.playerControl = true;

        tracyObject = (GameObject)Instantiate(tracyPreFab, spawnLocation + new Vector3(0.5f, 0, 0), rotation);
        tracyObject.name = "Tracy";
        tracy = tracyObject.GetComponent<PlayerController>();

        teamedUp = false;

        ChangeCameraTarget(sketchObject);
    }

    void ChangeCameraTarget(GameObject newTarget)
    {
        cam.SetTarget(newTarget.transform);
    }
}

