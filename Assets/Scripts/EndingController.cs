using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour {

    private string[] dialogue;

	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene().name.Equals("CaughtEnding")) {
            dialogue = new string[2];
            dialogue[1] = "After one of your companions was caught, the rest of you were quickly round up. The thieves tossed you";
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
