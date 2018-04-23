using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour {

    public bool isPickup, stayAfterPickup;

    private GameObject hintPanel;
    private Text hintText;
    public string hint;
    private StoryState storyState;


    // Use this for initialization
    void Awake() {

        hintText = GameObject.Find("Hint Text").GetComponent<Text>();
        hintPanel = GameObject.FindGameObjectWithTag("Hint");

        storyState = GameObject.FindGameObjectWithTag("StoryState").GetComponent<StoryState>();

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Q) && other.tag == "Player")
        {
            if (isPickup)
            {
                GameState.GetActiveCharacter().GetComponent<Inventory>().Add(gameObject.name);
                storyState.collected(gameObject.name);
                if (!stayAfterPickup)
                {
                    gameObject.SetActive(false);
                    hintPanel.SetActive(false);
                }
            } else
                //just a mouse or something
                gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (GameState.isTalking)
        {
            hintPanel.SetActive(true);
            hintText.text = "1 & 2: Conversation Replies\n E: Exit Conversation";
        }
        else if (other.tag == "Player" && GameState.GetActiveCharacter() == other.gameObject)
        {
            hintPanel.SetActive(true);
            hintText.text = hint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hintPanel.SetActive(false);
    }

}
