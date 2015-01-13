using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public string sceneToLoad;
    public bool isOpen = false;

    private Animator anim;
    private GameObject characterOpeningDoor;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OpenDoor(GameObject pCharacterOpeningDoor)
    {
        characterOpeningDoor = pCharacterOpeningDoor;
        anim.SetBool("OpenDoor", true);
        isOpen = true;
    }

    public void CloseDoor()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        anim.SetBool("OpenDoor", false);
        isOpen = false;

        // NEED TO WAIT TIL DOOR CLOSES BEFORE CHANGING SCENE
        //ChangeScene(); 
    }

    void ChangeScene()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
