using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
        print("collide");   
        if (other.gameObject.tag == "Player")
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
