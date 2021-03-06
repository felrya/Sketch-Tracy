﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Interactive Objects/Door")]
public class Door : MonoBehaviour
{
    public string sceneToLoad;
    public float sceneChangeTime = 1.0f;
    public bool isOpen = false;

    private bool changeScene = false;
    private float sceneChangeTimer;

    private Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        sceneChangeTimer = sceneChangeTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (changeScene)
        {
            if (sceneChangeTimer > 0)
            {
                sceneChangeTimer -= Time.deltaTime;
            }
            else
            {
                ChangeScene();
            }
        }
	}

    public void OpenDoor()
    {
        anim.SetBool("OpenDoor", true);
        isOpen = true;
    }

    public void CloseDoor()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 2;
        anim.SetBool("OpenDoor", false);
        isOpen = false;

        changeScene = true;
    }

    void ChangeScene()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
