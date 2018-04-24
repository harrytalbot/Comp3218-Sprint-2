using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingController : MonoBehaviour {

    private string[] dialogue;
    private GameObject dialogueBox;
    private GameObject button;
    private int current = 0;

	// Use this for initialization
	void Start () {
        dialogueBox = GameObject.Find("DialogueBox");
        button = GameObject.Find("ButtonTwo");

		if (SceneManager.GetActiveScene().name.Equals("HouseCaughtEnding") || SceneManager.GetActiveScene().name.Equals("ShedCaughtEnding")) {
            dialogue = new string[2];
            dialogue[0] = "After one of your companions was caught, the rest of you were quickly rounded up. The thieves tossed you out onto the street where you trod solemnly onwards.";
            dialogue[1] = "Disheartened, one by one your companions go their own way until you find yourself returning back to the farm, accepting your fate as a sold animal.";
        }
        else if (SceneManager.GetActiveScene().name.Equals("NoCompanionEnding")) {
            dialogue = new string[4];
            dialogue[0] = "On your travels to the city, you search for a place to sleep, you come across a big house with a spacious garage. Alas the residence is filled with robbers!";
            dialogue[1] = "Being on your own, you don't feel confident enough to try get rid of them, and settle under a small tree for the night.";
            dialogue[2] = "You continue to the city to follow your new found dream of being a musician. In the days that follow in the city you fail to rise to fame.";
            dialogue[3] = "You sigh as you wonder what could of been if you had been able to find like-minded companions...";
        }
        else if (SceneManager.GetActiveScene().name.Equals("HouseVictoryEnding")) {
            dialogue = new string[3];
            dialogue[0] = "Success! You and your friends were able to scare away all the robbers, safely assured that they definitely won't be coming back.";
            dialogue[1] = "The house you've taken is lovely and cozy, warm and inviting. In fact you all decide that you'll stay more than just one night!";
            dialogue[2] = "Many days pass, the group basking in the luxury of their new home. Some say that the group of musicians still reside there to this day!";
        }
        else if (SceneManager.GetActiveScene().name.Equals("ShedVictoryEnding")) {
            dialogue = new string[5];
            dialogue[0] = "Success! You and your friends were able to scare away all the robbers, safely assured that they definitely won't be coming back.";
            dialogue[1] = "The garage you've taken has just enough room for everyone to sleep is reasonable comfort.";
            dialogue[2] = "Come morning, you all leave the shed behind and trek on towards the city.";
            dialogue[3] = "It takes around 4 hours of travel before the three of you finally reach the city to start your music careers.";
            dialogue[4] = "The three of you rock the town, becoming the famous musicians you'd always dreamed of.";
        }

        dialogueBox.GetComponentInChildren<Text>().text = dialogue[0];
        button.GetComponentInChildren<Text>().text = "Continue";
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            current++;
            if (current < dialogue.Length)
                dialogueBox.GetComponentInChildren<Text>().text = dialogue[current];
            else if (SceneManager.GetActiveScene().name.Equals("HouseCaughtEnding"))
                SceneManager.LoadScene("House Level");
            else if (SceneManager.GetActiveScene().name.Equals("ShedCaughtEnding"))
                SceneManager.LoadScene("Shed Level");
            else if (SceneManager.GetActiveScene().name.Equals("NoCompanionEnding"))
                SceneManager.LoadScene("Main Menu");
            else if (SceneManager.GetActiveScene().name.Equals("HouseVictoryEnding"))
                SceneManager.LoadScene("Main Menu");
            else if (SceneManager.GetActiveScene().name.Equals("ShedVictoryEnding"))
                SceneManager.LoadScene("Main Menu");
        }
	}
}
