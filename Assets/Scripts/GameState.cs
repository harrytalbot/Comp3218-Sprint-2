using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public GameObject[] playerObjects = new GameObject[4];
    public static GameObject conversationUI;
    public static bool[] charactersGot = new bool[4];
    public static int activeCharacter;
    public static bool isTalking;
    public bool debug;
    
	void Awake () {
        charactersGot[0] = true;
        activeCharacter = 0;
        isTalking = false;
        conversationUI = GameObject.Find("Conversation UI");

        if (debug == true) {
            Debug.Log("Debug enabled. All characters unlocked.\nMay produce Errors if they aren't in the PlayerObjects list.");
            charactersGot[1] = true;
            charactersGot[2] = true;
            charactersGot[3] = true;
        }
	}

    void Update() {
        if (!isTalking) {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !(activeCharacter == 0) && charactersGot[0]) {
                SwapCharacter(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !(activeCharacter == 1) && charactersGot[1]) {
                SwapCharacter(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !(activeCharacter == 2) && charactersGot[2]) {
                SwapCharacter(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && !(activeCharacter == 3) && charactersGot[3]) {
                SwapCharacter(3);
            }
        }
    }

    void SwapCharacter(int characterNum) {
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = false;
        activeCharacter = characterNum;
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = true;
    }
}
