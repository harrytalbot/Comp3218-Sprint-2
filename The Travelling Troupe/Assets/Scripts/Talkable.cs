using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talkable : MonoBehaviour {

    public string message;
    private Text dialogueBox;
    private Button dialogueButton;
    private GameObject myUI;

    public void Awake() {
        dialogueBox = GameObject.Find("Dialogue").GetComponent<Text>();
        dialogueButton = GameObject.Find("Button").GetComponent<Button>();
        myUI = GameObject.Find("Player").GetComponent<InteractController>().myUI;
    }

    public void Interact() {
        dialogueBox.text = message;
        dialogueButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick() { // Follow up Dialogue or exiting from the talk situation can happen here.
        Debug.Log("THE BUTTON WAS CLICKED!");
        GameObject.Find("Player").GetComponent<PlayerController>().isTalking = false;
        myUI.SetActive(false);
    }
}
