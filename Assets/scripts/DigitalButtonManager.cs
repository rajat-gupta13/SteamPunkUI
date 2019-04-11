using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalButtonManager : MonoBehaviour {

    public GameObject oscInput;
    public Dropdown settingsDropdown;
	public Triggers triggers;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SettingsDropdown(int option){
        // called by the settings dropdown button
        //print(option);
        switch (option)
        {
            case 0:
                // settings
                break;
            case 1:
                // quit
                SendMessage("QuitGame");
                break;
            case 2:
                // blackout
                SendMessage("Blackout");
                break;
            case 3:
                // set lighting
                SendMessage("SetupLighting");
                break;
            case 4:
                // show osc input field
                oscInput.SetActive(true);
                break;
            case 5:
                // hide osc input field
                oscInput.SetActive(false);
                break;
        }
        settingsDropdown.value = 0;		// reset the button heading to "settings"
    }

	public void SomeButtonHit(string hit) {
		switch (hit) {
		case "someButton": 
			// send off a trigger
			triggers.Trigger ("Got-SOMETHING");
			break;
		case "playMovie":
			triggers.Trigger ("Got-playMovie");
			break;
		}
	}


}
