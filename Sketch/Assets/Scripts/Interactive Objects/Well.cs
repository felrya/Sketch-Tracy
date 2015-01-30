using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Interactive Objects/Well")]
public class Well : MonoBehaviour
{
    public string sceneToLoad;
    public float sceneChangeTime = 0.2f;

    private bool changeScene = false;
    private float sceneChangeTimer;

	// Use this for initialization
	void Start ()
    {
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

    public void EnterWell()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        changeScene = true;
    }

    void ChangeScene()
    {
        Application.LoadLevel(sceneToLoad);
    }
}
