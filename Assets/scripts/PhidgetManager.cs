using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhidgetManager : MonoBehaviour {

	private string[] data;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReceivedOSCmessage(string d){
		Debug.Log ("Received Phidget OSC : " + data);
		data = d.Split (' ');
		// data lines to be received:
		// /phidget IR code
		HandleMessage ();
	}

	void HandleMessage() {
		if (data.Length > 1) {
			// process /phidget messages
			if (data [0] == "/phidget") {
				// data has been received from a phidget OSC message
				// /phidget IR code

				// what is the function
				switch (data [1]) {
				case "IR":
					string code = data [2]; 
					// DO SOMETHING
					break;
				
				}
			}
		}
	}

}
