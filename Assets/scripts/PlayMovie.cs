using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayMovie : MonoBehaviour {



	private VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
		videoPlayer = GetComponent<VideoPlayer> ();
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayVideo() {
		this.gameObject.SetActive(true);
		videoPlayer.frame = 0;
		videoPlayer.Play ();
	}

	public void PlayVideo(VideoClip movie, bool loop) {
		this.gameObject.SetActive(true);
		videoPlayer.clip = movie;
		videoPlayer.isLooping = loop;
		videoPlayer.frame = 0;
		videoPlayer.Play ();
        if (!loop) Invoke("StopVideo", (float)movie.length);
	}

    public void StopVideo() {
        videoPlayer.Stop();
        this.gameObject.SetActive(false);
    }
}
