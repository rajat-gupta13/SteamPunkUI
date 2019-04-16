using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Triggers : MonoBehaviour
{

    public VideoClip[] videoFiles;
    public AudioClip[] audioFiles;

    public AudioSource audio;
    public PlayMovie video;
    public Lighting light;


    private string state;
    private OSCController osc;
    private int ledON = 100;
    private int ledOFF = 0;

    private DigitalButtonManager digitalButtons;
    private PhysicalButtonManager physicalButtons;
    // Use this for initialization
    void Start()
    {
        digitalButtons = GetComponent<DigitalButtonManager>();
        physicalButtons = GetComponent<PhysicalButtonManager>();
        state = "none";
        StartCoroutine(WaitToTrigger("Start", 1.0f)); // wait for everything to load
        osc = GameObject.Find("OSCMain").GetComponent<OSCController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log("turning all LEDs off");
        osc.SendOSCMessage("/phidget LED setAll 0");
    }

    public IEnumerator WaitToTrigger(string trigger, float delay)
    {
        yield return new WaitForSeconds(delay);
        Trigger(trigger);
    }

    public void Trigger(string trigger)
    {
        Debug.Log("Handel trigger - " + trigger);

        switch (trigger)
        {
            ////////////////////// INTRODUCTION ///////////////////////////////////////////////
            case "Start":
                if (state == "none")
                {
                    state = "preShow";
                    light.Blackout();
                    digitalButtons.startComm1 = true;
                }
                break;
            case "Got-incomingCall":
                if (state == "preShow")
                {
                    // user has .... DONE SOMETHING
                    state = "Intro";
                    // movie - load video#0
                    video.PlayVideo(videoFiles[0], false); // true => loop the video
                                                           // audio - play audio#0
                                                           //audio.clip = audioFiles[0];	// "audio name" <- add a comment to know what audio file is playing
                                                           //audio.Play ();
                                                           // lighting - orange all walls
                                                           //light.Light("/lighting fadeAdd SR1 255 128 0 0 255");
                    StartCoroutine(WaitToTrigger("Got-call1Ended", (float)videoFiles[0].length + 2.01f));
                }
                break;
            case "Got-call1Ended":
                //video.PlayVideo ();
                digitalButtons.startAsteroids = true;
                break;
            ///////////////////////// SYSTEM 1 /////////////////////////////////////////////
            case "Got-ShipHit":
                if (state == "Intro")
                {
                    // user has ...
                    state = "ShipHit";
                    // movie - load video#0
                    //video.PlayVideo (videoFiles[0], true);	// true => loop the video
                    // audio - play audio#0
                    audio.clip = audioFiles[0]; // "audio name" <- add a comment to know what audio file is playing
                    audio.Play();
                    // lighting - orange all walls
                    light.Light("/lighting fadeAdd SR1 255 0 0 0 255");
                    digitalButtons.sfx.clip = digitalButtons.sfxClips[15];
                    digitalButtons.sfx.Play();
                    StartCoroutine(WaitToTrigger("Got-Comm2", audioFiles[0].length + 0.01f));
                }
                break;

            case "Got-Comm2":
                if (state == "ShipHit")
                {
                    // user has ...
                    state = "Override";
                    // movie - load video#0
                    //video.PlayVideo (videoFiles[0], true);    // true => loop the video
                    // audio - play audio#0
                    audio.clip = audioFiles[1]; // "audio name" <- add a comment to know what audio file is playing
                    audio.Play();
                    // lighting - orange all walls
                    //light.Light("/lighting fadeAdd SR1 255 0 0 0 255");
                    digitalButtons.overrideAccess = true;
                    digitalButtons.decreaseO2 = true;
                }
                break;

            case "Got-ShipDocked":
                video.PlayVideo(videoFiles[1], false);
                digitalButtons.decreaseO2 = false;
                digitalButtons.pod1InUse = false;
                digitalButtons.pod2InUse = false;
                digitalButtons.pod1InUse = false;
                digitalButtons.pod4InUse = false;
                digitalButtons.pod5InUse = false;
                digitalButtons.shipRadarMoveSpeed = 0;
                digitalButtons.engine.Stop();
                light.Light("/lighting fadeAdd SR1 0 0 0 0 0");
                StartCoroutine(WaitToTrigger("Got-EndGame", (float)videoFiles[1].length + 0.01f));
                break;

            case "Got-EndGame":
                StartCoroutine(digitalButtons.Die());
                break;
            ///////////////////////// POSSIBLE PHIDGET /////////////////////////////////////////////
            case "Got-Toggle1":
                // turn on LED feedback
                LEDFeedback("Toggle1", ledON);
                digitalButtons.toggle1 = true;
                digitalButtons.pod1InUse = true;
                digitalButtons.sfx.clip = digitalButtons.sfxClips[0];
                digitalButtons.sfx.Play();
                if (digitalButtons.pod2InUse || digitalButtons.pod3InUse || digitalButtons.pod4InUse || digitalButtons.pod5InUse)
                {
                    digitalButtons.pod2InUse = false;
                    digitalButtons.pod3InUse = false;
                    digitalButtons.pod4InUse = false;
                    digitalButtons.pod5InUse = false;
                    audio.clip = audioFiles[2];
                    audio.Play();
                }
                break;
            case "Got-Toggle2":
                // turn on LED feedback
                LEDFeedback("Toggle2", ledON);
                digitalButtons.pod2InUse = true;
                digitalButtons.toggle2 = true;
                digitalButtons.sfx.clip = digitalButtons.sfxClips[1];
                digitalButtons.sfx.Play();
                if (digitalButtons.pod1InUse || digitalButtons.pod3InUse || digitalButtons.pod4InUse || digitalButtons.pod5InUse)
                {
                    digitalButtons.pod1InUse = false;
                    digitalButtons.pod3InUse = false;
                    digitalButtons.pod4InUse = false;
                    digitalButtons.pod5InUse = false;
                    audio.clip = audioFiles[2];
                    audio.Play();
                }
                break;
            case "Got-Toggle3":
                // turn on LED feedback
                LEDFeedback("Toggle3", ledON);
                digitalButtons.toggle3 = true;
                digitalButtons.pod3InUse = true;
                digitalButtons.sfx.clip = digitalButtons.sfxClips[2];
                digitalButtons.sfx.Play();
                if (digitalButtons.pod2InUse || digitalButtons.pod1InUse || digitalButtons.pod4InUse || digitalButtons.pod5InUse)
                {
                    digitalButtons.pod2InUse = false;
                    digitalButtons.pod1InUse = false;
                    digitalButtons.pod4InUse = false;
                    digitalButtons.pod5InUse = false;
                    audio.clip = audioFiles[2];
                    audio.Play();
                }
                break;
            case "Got-Toggle4":
                // turn on LED feedback
                LEDFeedback("Toggle4", ledON);
                digitalButtons.pod4InUse = true;
                digitalButtons.sfx.clip = digitalButtons.sfxClips[3];
                digitalButtons.sfx.Play();
                if (digitalButtons.pod2InUse || digitalButtons.pod1InUse || digitalButtons.pod3InUse || digitalButtons.pod5InUse)
                {
                    digitalButtons.pod2InUse = false;
                    digitalButtons.pod3InUse = false;
                    digitalButtons.pod1InUse = false;
                    digitalButtons.pod5InUse = false;
                    audio.clip = audioFiles[2];
                    audio.Play();
                }
                digitalButtons.toggle4 = true;
                break;
            case "Got-Toggle5":
                // turn on LED feedback
                LEDFeedback("Toggle5", ledON);
                digitalButtons.pod5InUse = true;
                digitalButtons.sfx.clip = digitalButtons.sfxClips[4];
                digitalButtons.sfx.Play();
                if (digitalButtons.pod2InUse || digitalButtons.pod1InUse || digitalButtons.pod3InUse || digitalButtons.pod4InUse)
                {
                    digitalButtons.pod2InUse = false;
                    digitalButtons.pod3InUse = false;
                    digitalButtons.pod4InUse = false;
                    digitalButtons.pod1InUse = false;
                    audio.clip = audioFiles[2];
                    audio.Play();
                }
                digitalButtons.toggle5 = true;
                break;
            case "Lost-Toggle1":
                // turn off LED feedback
                LEDFeedback("Toggle1", ledOFF);
                digitalButtons.toggle1 = false;
                digitalButtons.pod1InUse = false;
                break;
            case "Lost-Toggle2":
                // turn off LED feedback
                LEDFeedback("Toggle2", ledOFF);
                digitalButtons.toggle2 = false;
                digitalButtons.pod2InUse = false;
                break;
            case "Lost-Toggle3":
                // turn off LED feedback
                LEDFeedback("Toggle3", ledOFF);
                digitalButtons.toggle3 = false;
                digitalButtons.pod3InUse = false;
                break;
            case "Lost-Toggle4":
                // turn off LED feedback
                LEDFeedback("Toggle4", ledOFF);
                digitalButtons.toggle4 = false;
                digitalButtons.pod4InUse = false;
                break;
            case "Lost-Toggle5":
                // turn off LED feedback
                LEDFeedback("Toggle5", ledOFF);
                digitalButtons.toggle5 = false;
                digitalButtons.pod5InUse = false;
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
                LEDFeedback("CoverToggle1", ledON);

                break;
            case "Got-CoverToggle2":
                // turn on LED feedback
                LEDFeedback("CoverToggle2", ledON);

                break;
            case "Got-CoverToggle3":
                // turn on LED feedback
                LEDFeedback("CoverToggle3", ledON);

                break;
            case "Lost-CoverToggle1":
                // turn off LED feedback
                LEDFeedback("CoverToggle1", ledOFF);

                break;
            case "Lost-CoverToggle2":
                // turn off LED feedback
                LEDFeedback("CoverToggle2", ledOFF);

                break;
            case "Lost-CoverToggle3":
                // turn off LED feedback
                LEDFeedback("CoverToggle3", ledOFF);

                break;
            case "Got-Key":

                break;
            case "Lost-Key":

                break;
            case "Got-AlClip1":
                // turn on LED feedback
                digitalButtons.alClip1 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip1", ledON);

                break;
            case "Got-AlClip2":
                // turn on LED feedback
                digitalButtons.alClip2 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip2", ledON);

                break;
            case "Got-AlClip3":
                // turn on LED feedback
                digitalButtons.alClip3 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip3", ledON);

                break;
            case "Got-AlClip4":
                // turn on LED feedback
                digitalButtons.alClip4 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip4", ledON);

                break;
            case "Lost-AlClip1":
                // turn off LED feedback
                digitalButtons.alClip1 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip1", ledOFF);

                break;
            case "Lost-AlClip2":
                // turn off LED feedback
                digitalButtons.alClip2 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip2", ledOFF);

                break;
            case "Lost-AlClip3":
                // turn off LED feedback
                digitalButtons.alClip3 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip3", ledOFF);

                break;
            case "Lost-AlClip4":
                // turn off LED feedback
                digitalButtons.alClip4 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("AlClip4", ledOFF);

                break;
            case "Got-Knife1":
                // turn on LED feedback
                digitalButtons.knife1 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife1", ledON);

                break;
            case "Got-Knife2":
                // turn on LED feedback
                digitalButtons.knife2 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife2", ledON);

                break;
            case "Got-Knife3":
                // turn on LED feedback
                digitalButtons.knife3 = true;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife3", ledON);

                break;
            case "Lost-Knife1":
                // turn off LED feedback
                digitalButtons.knife1 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife1", ledOFF);

                break;
            case "Lost-Knife2":
                // turn off LED feedback
                digitalButtons.knife2 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife2", ledOFF);

                break;
            case "Lost-Knife3":
                // turn off LED feedback
                digitalButtons.knife3 = false;
                StartCoroutine(digitalButtons.Gyroscope());
                LEDFeedback("Knife3", ledOFF);

                break;
            case "Got-cirTouchTouch":
                // when you receive a touch event you dont get the value so request it
                osc.SendOSCMessage("/phidget cirTouch getSensor");
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
                LEDFeedback("Panel1", ledON);
                break;
            case "Off-Panel1":
                // turn off LED feedback for Panel1
                LEDFeedback("Panel1", ledOFF);
                break;
            case "On-Panel2":
                // turn on LED feedback for Panel2
                LEDFeedback("Panel2", ledON);
                break;
            case "Off-Panel2":
                // turn off LED feedback for Panel2
                LEDFeedback("Panel2", ledOFF);
                break;
            case "On-Panel3":
                // turn on LED feedback for Panel3
                LEDFeedback("Panel3", ledON);
                break;
            case "Off-Panel3":
                // turn off LED feedback for Panel3
                LEDFeedback("Panel3", ledOFF);
                break;
            case "On-LeftLight":
                // turn on LED feedback for LeftLight
                LEDFeedback("LeftLight", ledON);
                break;
            case "Off-LeftLight":
                // turn off LED feedback for LeftLight
                LEDFeedback("LeftLight", ledOFF);
                break;
            case "On-RightLight":
                // turn on LED feedback for RightLight
                LEDFeedback("RightLight", ledON);
                break;
            case "Off-RightLight":
                // turn off LED feedback for RightLight
                LEDFeedback("RightLight", ledOFF);
                break;

                ////////////////////// END CASES ///////////////////////////////////////////////
        }
    }

    void LEDFeedback(string tag, int value)
    {
        osc.SendOSCMessage("/phidget LED setTag " + tag + " " + value.ToString());
    }

}
