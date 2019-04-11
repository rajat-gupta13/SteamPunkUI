using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Triggers : MonoBehaviour {

	public VideoClip[] videoFiles;
	public AudioClip[] audioFiles;

	public AudioSource audio;
	public PlayMovie video;
	public Lighting light;


	private string state;
	private OSCController osc;
	private int ledON = 100;
	private int ledOFF = 0;

	// Use this for initialization
	void Start () {
		state = "none";
		StartCoroutine (WaitToTrigger ("Start", 1.0f)); // wait for everything to load
		osc = GameObject.Find ("OSCMain").GetComponent<OSCController> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnDisable () {
		Debug.Log("turning all LEDs off");
		osc.SendOSCMessage ("/phidget LED setAll 0");
	}

	IEnumerator WaitToTrigger(string trigger, float delay) {
		yield return new WaitForSeconds (delay);
		Trigger (trigger);
	}

	public void Trigger(string trigger) {
		Debug.Log ("Handel trigger - " + trigger);

		switch (trigger) {
		////////////////////// INTRODUCTION ///////////////////////////////////////////////
		case "Start":
			if (state == "none") {
				state = "preShow";
				light.Blackout ();
			}
			break;
		case "Got-floorButtonUSC":
			if (state == "preShow") {
				// user has .... DONE SOMETHING
				state = "Intro";
				// movie - load video#0
				video.PlayVideo (videoFiles[0], true);	// true => loop the video
				// audio - play audio#0
				audio.clip = audioFiles[0];	// "audio name" <- add a comment to know what audio file is playing
				audio.Play ();
				// lighting - orange all walls
				light.Light("/lighting fadeAdd SR1 255 128 0 0 255");
			}
			break;
		case "Got-playMovie":
			video.PlayVideo ();
			break;
		///////////////////////// SYSTEM 1 /////////////////////////////////////////////
		case "Got-SOMETHING":
			if (state == "SOMETHING") {
				// user has ...
				state = "SOMETHING ELSE";
				// movie - load video#0
				video.PlayVideo (videoFiles[0], true);	// true => loop the video
				// audio - play audio#0
				audio.clip = audioFiles[0];	// "audio name" <- add a comment to know what audio file is playing
				audio.Play ();
				// lighting - orange all walls
				light.Light("/lighting fadeAdd SR1 255 128 0 0 255");
			}
			break;
		///////////////////////// POSSIBLE PHIDGET /////////////////////////////////////////////
		case "Got-Toggle1":
			// turn on LED feedback
			LEDFeedback ("Toggle1", ledON);

			break;
		case "Got-Toggle2":
			// turn on LED feedback
			LEDFeedback ("Toggle2", ledON);

			break;
		case "Got-Toggle3":
			// turn on LED feedback
			LEDFeedback ("Toggle3", ledON);

			break;
		case "Got-Toggle4":
			// turn on LED feedback
			LEDFeedback ("Toggle4", ledON);

			break;
		case "Got-Toggle5":
			// turn on LED feedback
			LEDFeedback ("Toggle5", ledON);

			break;
		case "Lost-Toggle1":
			// turn off LED feedback
			LEDFeedback ("Toggle1", ledOFF);

			break;
		case "Lost-Toggle2":
			// turn off LED feedback
			LEDFeedback ("Toggle2", ledOFF);

			break;
		case "Lost-Toggle3":
			// turn off LED feedback
			LEDFeedback ("Toggle3", ledOFF);

			break;
		case "Lost-Toggle4":
			// turn off LED feedback
			LEDFeedback ("Toggle4", ledOFF);

			break;
		case "Lost-Toggle5":
			// turn off LED feedback
			LEDFeedback ("Toggle5", ledOFF);

			break;
		/*case "Got-Hole1":
			// turn on LED feedback
			LEDFeedback ("Hole1", ledON);

			break;
		case "Got-Hole2":
			// turn on LED feedback
			LEDFeedback ("Hole2", ledON);

			break;
		case "Lost-Hole1":
			// turn off LED feedback
			LEDFeedback ("Hole1", ledOFF);

			break;
		case "Lost-Hole2":
			// turn off LED feedback
			LEDFeedback ("Hole2", ledOFF);

			break;*/
		case "Got-CoverToggle1":
			// turn on LED feedback
			LEDFeedback ("CoverToggle1", ledON);

			break;
		case "Got-CoverToggle2":
			// turn on LED feedback
			LEDFeedback ("CoverToggle2", ledON);

			break;
		case "Got-CoverToggle3":
			// turn on LED feedback
			LEDFeedback ("CoverToggle3", ledON);

			break;
		case "Lost-CoverToggle1":
			// turn off LED feedback
			LEDFeedback ("CoverToggle1", ledOFF);

			break;
		case "Lost-CoverToggle2":
			// turn off LED feedback
			LEDFeedback ("CoverToggle2", ledOFF);

			break;
		case "Lost-CoverToggle3":
			// turn off LED feedback
			LEDFeedback ("CoverToggle3", ledOFF);

			break;
		case "Got-Key":

			break;
		case "Lost-Key":

			break;
		case "Got-AlClip1":
			// turn on LED feedback
			LEDFeedback ("AlClip1", ledON);

			break;
		case "Got-AlClip2":
			// turn on LED feedback
			LEDFeedback ("AlClip2", ledON);

			break;
		case "Got-AlClip3":
			// turn on LED feedback
			LEDFeedback ("AlClip3", ledON);

			break;
		case "Got-AlClip4":
			// turn on LED feedback
			LEDFeedback ("AlClip4", ledON);

			break;
		case "Lost-AlClip1":
			// turn off LED feedback
			LEDFeedback ("AlClip1", ledOFF);

			break;
		case "Lost-AlClip2":
			// turn off LED feedback
			LEDFeedback ("AlClip2", ledOFF);

			break;
		case "Lost-AlClip3":
			// turn off LED feedback
			LEDFeedback ("AlClip3", ledOFF);

			break;
		case "Lost-AlClip4":
			// turn off LED feedback
			LEDFeedback ("AlClip4", ledOFF);

			break;
		case "Got-Knife1":
			// turn on LED feedback
			LEDFeedback ("Knife1", ledON);

			break;
		case "Got-Knife2":
			// turn on LED feedback
			LEDFeedback ("Knife2", ledON);

			break;
		case "Got-Knife3":
			// turn on LED feedback
			LEDFeedback ("Knife3", ledON);

			break;
		case "Lost-Knife1":
			// turn off LED feedback
			LEDFeedback ("Knife1", ledOFF);

			break;
		case "Lost-Knife2":
			// turn off LED feedback
			LEDFeedback ("Knife2", ledOFF);

			break;
		case "Lost-Knife3":
			// turn off LED feedback
			LEDFeedback ("Knife3", ledOFF);

			break;
		case "Got-cirTouchTouch":
			// when you receive a touch event you dont get the value so request it
			osc.SendOSCMessage ("/phidget cirTouch getSensor");
			// turn on LED feedback
			//LEDFeedback ("CirTouch", ledON);

			break;
		case "Lost-cirTouchTouch":
			// turn off LED feedback
			//LEDFeedback ("CirTouch", ledOFF);

			break;
		case "Got-cirTouchNear":

			break;
		case "Lost-cirTouchNear":

			break;
		case "Got-circularTouch0":

			break;
		case "Got-circularTouch1":

			break;
		case "Got-circularTouch2":

			break;
		case "Got-circularTouch3":

			break;
		case "Got-circularTouch4":

			break;
		case "Got-circularTouch5":

			break;
		case "Got-circularTouch6":

			break;
		case "Got-circularTouch7":

			break;
		case "Got-circularTouch8":

			break;
		case "Got-circularTouch9":

			break;

		////////////////////// UTILITY ///////////////////////////////////////////////
		case "On-Panel1":
			// turn on LED feedback for Panel1
			LEDFeedback ("Panel1", ledON);
			break;
		case "Off-Panel1":
			// turn off LED feedback for Panel1
			LEDFeedback ("Panel1", ledOFF);
			break;
		case "On-Panel2":
			// turn on LED feedback for Panel2
			LEDFeedback ("Panel2", ledON);
			break;
		case "Off-Panel2":
			// turn off LED feedback for Panel2
			LEDFeedback ("Panel2", ledOFF);
			break;
		case "On-Panel3":
			// turn on LED feedback for Panel3
			LEDFeedback ("Panel3", ledON);
			break;
		case "Off-Panel3":
			// turn off LED feedback for Panel3
			LEDFeedback ("Panel3", ledOFF);
			break;
		case "On-LeftLight":
			// turn on LED feedback for LeftLight
			LEDFeedback ("LeftLight", ledON);
			break;
		case "Off-LeftLight":
			// turn off LED feedback for LeftLight
			LEDFeedback ("LeftLight", ledOFF);
			break;
		case "On-RightLight":
			// turn on LED feedback for RightLight
			LEDFeedback ("RightLight", ledON);
			break;
		case "Off-RightLight":
			// turn off LED feedback for RightLight
			LEDFeedback ("RightLight", ledOFF);
			break;

		////////////////////// END CASES ///////////////////////////////////////////////
		}
	}

	void LEDFeedback(string tag, int value) {
		osc.SendOSCMessage ("/phidget LED setTag " + tag + " " + value.ToString());
	}

}
