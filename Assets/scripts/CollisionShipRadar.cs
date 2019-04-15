using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShipRadar : MonoBehaviour {

    public DigitalButtonManager digitalButtons;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Ship")
        {
            digitalButtons.asteroidsHit = true;
        }
    }
}
