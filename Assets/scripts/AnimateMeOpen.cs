using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMeOpen : MonoBehaviour {

    public Animator anim;
    public string parameterName;

    private bool amOpen;
    
    // Use this for initialization
	void Start () {
        amOpen = true;
        AnimateMe();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AnimateMe() {
        if (amOpen)
        {
            // close me
            amOpen = false;
            anim.SetBool(parameterName, amOpen);
        }
        else
        {
            // open me
            amOpen = true;
            anim.SetBool(parameterName, amOpen);
        }

        // this could have been done this way too

        // amOpen = !amOpen;
        // anim.SetBool(parameterName, amOpen);

    }

}
