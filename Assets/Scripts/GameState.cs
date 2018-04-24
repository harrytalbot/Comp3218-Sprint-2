using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    private static GameObject[] playerObjects = new GameObject[4];
    private static GameObject[] inventoryIcons;
    private static GameObject[] enemies; 

    public static GameObject conversationUI;
    public static GameObject hintUI;

    public int initialCharacter;
    private static bool[] charactersGot = new bool[4];
    public static int activeCharacter;
    public static bool isTalking;

    public bool character1Active;
    public bool character2Active;
    public bool character3Active;
    public bool character4Active;

    public bool canControlOtherCharacters = false;

    public bool debug;
    public static bool firstRun = false;
    public float delayTime;

    public GameObject[] tempObjects;
    
    void Awake () {
        if (initialCharacter == 0)
            character1Active = true;
        else if (initialCharacter == 1)
            character2Active = true;
        else if (initialCharacter == 2)
            character3Active = true;
        else if (initialCharacter == 3)
            character4Active = true;

        if (SceneManager.GetActiveScene().name.Contains("House") || SceneManager.GetActiveScene().name.Contains("Shed")) { 
            enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
        } 

        charactersGot[initialCharacter] = true;
        activeCharacter = initialCharacter;
        isTalking = false;
        conversationUI = GameObject.FindGameObjectWithTag("DialogueBox");
        hintUI = GameObject.FindGameObjectWithTag("Hint");

        tempObjects = GameObject.FindGameObjectsWithTag("Player");
        inventoryIcons = GameObject.FindGameObjectsWithTag("Item");

        for (int i = 0; i < tempObjects.Length; i++) {
            for (int j = 0; j < tempObjects.Length; j++) {
                if (tempObjects[j].GetComponent<PlayerController>().characterNumber == i) {
                    playerObjects[i] = tempObjects[j];
                    Debug.Log(tempObjects[j] + " has characterNumber = " + (tempObjects[j].GetComponent<PlayerController>().characterNumber));
                }
            }
        }

        for (int i = 0; i < playerObjects.Length; i++) {
            if (playerObjects[i] != null) {
                playerObjects[i].GetComponent<PlayerController>().mainCam.enabled = false;
                playerObjects[i].GetComponentInChildren<AudioListener>().enabled = false;
                if (SceneManager.GetActiveScene().name.Equals("Shed Level"))
                    playerObjects[i].SetActive(false);
            }
        }
        GameObject player = playerObjects[initialCharacter];
        Debug.Log("Variable Player = " + player);
        Inventory inventory = player.GetComponent<Inventory>();
        for (int i = 0; i < inventoryIcons.Length; i++) {
            if (inventoryIcons[i] != null) {
                if (!inventory.GotItem(inventoryIcons[i].name)) {
                    inventoryIcons[i].SetActive(false);
                }
            }
        }
        player.GetComponent<PlayerController>().mainCam.enabled = true;
        player.GetComponentInChildren<AudioListener>().enabled = true;

        if (debug == true) {
            Debug.Log("Debug enabled. All characters unlocked.\nMay produce Errors if they aren't tagged with 'Player'.");
            charactersGot[0] = true;
            charactersGot[1] = true;
            charactersGot[2] = true;
            charactersGot[3] = true;
        }   
        else if (firstRun) {
            charactersGot[0] = character1Active;
            charactersGot[1] = character2Active;
            charactersGot[2] = character3Active;
            charactersGot[3] = character4Active;
        }
        if (SceneManager.GetActiveScene().name.Equals("Shed Level")) {
            playerObjects[0].SetActive(charactersGot[0]);
            playerObjects[1].SetActive(charactersGot[1]);
            playerObjects[2].SetActive(charactersGot[2]);
        }
        firstRun = false;
	}

    void Update() {
        if (!isTalking && canControlOtherCharacters) {
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
        if (SceneManager.GetActiveScene().name.Contains("Shed") || SceneManager.GetActiveScene().name.Contains("House")) {
            bool done = true;
            foreach (GameObject enemy in enemies) {
                if (enemy.GetComponentInChildren<EnemyVisionCone>().enabled == true)
                    done = false;
            }
        if (done && delayTime > 0)
            delayTime -= Time.deltaTime;
        else if (done) {
                if (SceneManager.GetActiveScene().name.Contains("Shed"))
                    Initiate.Fade("ShedVictoryEnding", Color.black, 1);
                else if (SceneManager.GetActiveScene().name.Contains("House"))
                    Initiate.Fade("HouseVictoryEnding", Color.black, 1);
            }            
        }
    }

    private void SwapCharacter(int characterNum) {
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = false;
        playerObjects[activeCharacter].GetComponentInChildren<AudioListener>().enabled = false;
        activeCharacter = characterNum;
        playerObjects[activeCharacter].GetComponent<PlayerController>().mainCam.enabled = true;
        playerObjects[activeCharacter].GetComponentInChildren<AudioListener>().enabled = true;
    }

    public static GameObject GetActiveCharacter() {
        return playerObjects[activeCharacter];
    }

    public static GameObject[] GetCharacters() {
        return playerObjects;
    }

    public static bool IsUnlocked(GameObject character) {
      
        for (int i = 0; i < 4; i++) {
            if (playerObjects[i] != null && playerObjects[i].Equals(character) && charactersGot[i])
                return true;
        }
        return false;
    }

    public static void UnlockCharacter(int character) {
        charactersGot[character] = true;
    }

    public static void LockCharacter(int character)
    {
        charactersGot[character] = false;
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
