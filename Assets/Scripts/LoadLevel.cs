using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadLevel : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other)
    {
        int count = 0;
        if (other.gameObject.tag == "Player")
        {
            GameObject[] characters = GameState.GetCharacters();
            foreach (GameObject current in characters) {
                if (GameState.IsUnlocked(current))
                    count++;
            }
            if (count == 4)
                SceneManager.LoadScene("House Level", LoadSceneMode.Single);
            else if (count == 1)
                Initiate.Fade("NoCompanionEnding", Color.black, 1);
            else
                SceneManager.LoadScene("Shed Level", LoadSceneMode.Single);
        }
    }
}