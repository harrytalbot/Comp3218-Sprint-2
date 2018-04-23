using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour {

    private string[] dialogue;
    private GameObject dialogueBox;

	// Use this for initialization
	void Start () {
        dialogueBox = GameObject.Find("DialogueBox");

		if (SceneManager.GetActiveScene().name.Equals("CaughtEnding")) {
            dialogue = new string[2];
            dialogue[0] = "After one of your companions was caught, the rest of you were quickly rounded up. The thieves tossed you out onto the street where you trod solemnly onwards.";
            dialogue[1] = "Disheartened, one by one your companions go their own way until you find yourself returning back to the farm, accepting your fate as a sold animal.";
        }
        else if (SceneManager.GetActiveScene().name.Equals("NoCompanionEnding")) {
            dialogue = new string[2];
            dialogue[0] = "You ";
            dialogue[1] = "";
        }
        else if (SceneManager.GetActiveScene().name.Equals("HouseVictoryEnding")) {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
