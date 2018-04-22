﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private static GameObject[] playerObjects = new GameObject[4];
    private static GameObject[] inventoryIcons;
    public static GameObject conversationUI;
    public GameObject convUIForInspector;
    private static bool[] charactersGot = new bool[4];
    public static int activeCharacter;
    public static bool isTalking;
    public int initialCharacter;
    public bool character1Active = false;
    public bool character2Active = false;
    public bool character3Active = false;
    public bool character4Active = false;
    public bool debug;
    
	void Awake () {
        if (initialCharacter == 0)
            character1Active = true;
        else if (initialCharacter == 1)
            character2Active = true;
        else if (initialCharacter == 2)
            character3Active = true;
        else if (initialCharacter == 3)
            character4Active = true;

        charactersGot[initialCharacter] = true;
        activeCharacter = initialCharacter;
        isTalking = false;
        conversationUI = GameObject.FindGameObjectWithTag("DialogueBox");
        convUIForInspector = conversationUI;
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Player");
        inventoryIcons = GameObject.FindGameObjectsWithTag("Item");

        for (int i = 0; i < tempObjects.Length; i++) {
            for (int j = 0; j < tempObjects.Length; j++) {
                if (tempObjects[j].GetComponent<PlayerController>().characterNumber - 1 == i) {
                    playerObjects[i] = tempObjects[j];
                    Debug.Log(tempObjects[j]);
                }
            }
        }

        for (int i = 0; i < playerObjects.Length; i++) {
            if (playerObjects[i] != null) {
                playerObjects[i].GetComponent<PlayerController>().mainCam.enabled = false;
            }
        }
        GameObject player = playerObjects[initialCharacter];
        Inventory inventory = player.GetComponent<Inventory>();
        for (int i = 0; i < inventoryIcons.Length; i++) {
            if (inventoryIcons[i] != null) {
                if (!inventory.GotItem(inventoryIcons[i].name)) {
                    inventoryIcons[i].SetActive(false);
                }
            }
        }
        player.GetComponent<PlayerController>().mainCam.enabled = true;

        if (debug == true) {
            Debug.Log("Debug enabled. All characters unlocked.\nMay produce Errors if they aren't tagged with 'Player'.");
            charactersGot[0] = true;
            charactersGot[1] = true;
            charactersGot[2] = true;
            charactersGot[3] = true;
        }
        else {
            charactersGot[0] = character1Active;
            charactersGot[1] = character2Active;
            charactersGot[2] = character3Active;
            charactersGot[3] = character4Active;
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
            else if (Input.GetKeyDown(KeyCode.Alpha0)) {
                UnlockCharacter(0); UnlockCharacter(1); UnlockCharacter(2); UnlockCharacter(3);
            }
        }
    }

    private void SwapCharacter(int characterNum) {
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = false;
        activeCharacter = characterNum;
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = true;
    }

    public static GameObject GetActiveCharacter() {
        return playerObjects[activeCharacter];
    }

    public static bool IsUnlocked(int characterNumber) {

        return charactersGot[characterNumber-1];
        /*
        for (int i = 0; i < 4; i++) {
            if (playerObjects[i] != null && playerObjects[i].Equals(character) && charactersGot[i])
                return true;
        }
        return false; */
    }

    public static void UnlockCharacter(int character) {
        charactersGot[character] = true;
    }

    public static void UpdateIcon(Inventory inventory) {
        for (int i = 0; i < inventoryIcons.Length; i++) {
            if (inventoryIcons[i] != null) {
                if (inventory.GotItem(inventoryIcons[i].name)) {
                    inventoryIcons[i].SetActive(true);
                }
            }
        }
    }
}
