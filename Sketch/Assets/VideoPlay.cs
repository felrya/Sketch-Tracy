using UnityEngine;
using System.Collections;

public class VideoPlay : MonoBehaviour
{
    public MovieTexture movie;

	void Start ()
    {
        renderer.material.mainTexture = movie;
        movie.loop = true;
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
	}
}
