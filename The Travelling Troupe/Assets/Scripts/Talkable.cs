using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talkable : MonoBehaviour {

    public string message;
    private Text dialogueBox;
    private Button dialogueButtonOne, dialogueButtonTwo;
    private GameObject myUI;

    public int convEntryPoint;
    private int convPoint, convNextPoint;
    public Conversation conversation;

    public void Awake() {
        
        dialogueBox = GameObject.Find("Dialogue").GetComponent<Text>();
        // two buttons for responses
        dialogueButtonOne = GameObject.Find("ButtonOne").GetComponent<Button>();
        dialogueButtonTwo = GameObject.Find("ButtonTwo").GetComponent<Button>();

        myUI = GameObject.Find("Player").GetComponent<InteractController>().myUI;
        convNextPoint = convEntryPoint;

    }

    void FixedUpdate() {
        // if these have changed, the user has selected an option
        if (convNextPoint != convPoint)
        {
            convPoint = convNextPoint;
            dialogueBox.text = conversation.getNodes()[convNextPoint].getMessage();
            int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointer();
            string[] replies = conversation.getNodes()[convNextPoint].getReplies();
            dialogueButtonOne.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
            dialogueButtonTwo.GetComponentInChildren<Text>().text = (replies[1] + "(" + replyPointers[1] + ")");

        }
    }

    /**
     * Method for initial interaction, sets up the dialog and gets the conversation pointers right so fixedUpdae
     * is ready to see a change
     **/
    public void Interact() {
        convNextPoint = convEntryPoint;
        dialogueBox.text = conversation.getNodes()[convNextPoint].getMessage();
        int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointer();
        string[] replies = conversation.getNodes()[convNextPoint].getReplies();
        dialogueButtonOne.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
        dialogueButtonTwo.GetComponentInChildren<Text>().text = (replies[1] + "(" + replyPointers[1] + ")");
        //dialogueButtonOne.onClick.AddListener(TaskOnClick);
    }

    //unused    
    void TaskOnClick() { // Follow up Dialogue or exiting from the talk situation can happen here.
        Debug.Log("THE BUTTON WAS CLICKED!");
        GameObject.Find("Player").GetComponent<PlayerController>().isTalking = false;
        myUI.SetActive(false);
    }

    /** 
     * Method to convert the user's reply selection to the actual reply
     * 
     */
    public void setReply(int reply)
    {
        if (reply == -2)
        {
            // conversation has been cancelled. kill it here rather than waiting for update.
            GameObject.Find("Player").GetComponent<PlayerController>().isTalking = false;
            myUI.SetActive(false);
            convNextPoint = convPoint;
            return;
        }

        // get the response
        int temp = conversation.getNodes()[convPoint].getReplyPointer()[reply];
        if (temp == -1)
        {
            // conversation has ended. kill it here rather than waiting for update.
            GameObject.Find("Player").GetComponent<PlayerController>().isTalking = false;
            myUI.SetActive(false);
            convNextPoint = convPoint;
        }
        else
        {
            //update the response so fixedUpdate will see the difference
            convNextPoint = temp;
        }
    }

}
