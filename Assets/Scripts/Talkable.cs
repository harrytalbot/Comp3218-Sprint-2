    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talkable : MonoBehaviour {

    public bool isPickUp;
    public bool stayAfterPickUp = true;
    public int pickUpIndex;

    // text to display when near the talkable, and also the talkable name
    public string hint, talkableName;
    public int convEntryPoint;
    public Conversation conversation;

    // text objects of the panels
    private Text dialogueText, nameText, buttonOneText, buttonTwoText, hintText;
    private GameObject buttonOne, buttonTwo;
    private GameObject dialoguePanel, hintPanel;

    // the current conversation poin and the next one
    private int convPoint, convNextPoint;

    // script to run if the conversation is a success.
    public GameObject successfullCoversationScript;

    public void Awake() {
        
        dialogueText = GameObject.Find("Dialogue").GetComponent<Text>();
        nameText = GameObject.Find("Name").GetComponent<Text>();
        // two buttons for responses
        buttonOne = GameObject.Find("ButtonOne");
        buttonTwo = GameObject.Find("ButtonTwo");
        buttonOneText = buttonOne.GetComponent<Button>().GetComponentInChildren<Text>();
        buttonTwoText = buttonTwo.GetComponent<Button>().GetComponentInChildren<Text>();
        hintText = GameObject.Find("Hint Text").GetComponent<Text>();

        dialoguePanel = GameObject.FindGameObjectWithTag("DialogueBox");
        hintPanel = GameObject.FindGameObjectWithTag("Hint");
        convNextPoint = convEntryPoint;

    }

    void FixedUpdate() {
        // if these have changed, the user has selected an option
        if (convNextPoint != convPoint)
        {
            convPoint = convNextPoint;
            dialogueText.text = conversation.getNodes()[convNextPoint].getMessage();
            int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointer();
            string[] replies = conversation.getNodes()[convNextPoint].getReplies();
            buttonOneText.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
            if (replies.Length > 1)
            {
                buttonTwo.SetActive(true);
                buttonTwoText.text = (replies[1] + "(" + replyPointers[1] + ")");
            } else
            {
                buttonTwo.SetActive(false);
            }


        }
    }

    /**
     * Method for initial interaction, sets up the dialog and gets the conversation pointers right so fixedUpdate
     * is ready to see a change
     **/
    public void Interact() {
        dialogueText.text = conversation.getNodes()[convNextPoint].getMessage();
        nameText.text = talkableName;
        int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointer();
        string[] replies = conversation.getNodes()[convNextPoint].getReplies();
        buttonOneText.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
        if (replies.Length > 1)
        {
            buttonTwo.SetActive(true);
            buttonTwoText.GetComponentInChildren<Text>().text = (replies[1] + "(" + replyPointers[1] + ")");
        }
        else
        {
            buttonTwo.SetActive(false);
        }
        //dialogueButtonOne.onClick.AddListener(TaskOnClick);
    }

    /** 
     * Method to convert the user's reply selection to the actual reply
     */
    public void setReply(int reply)
    {
        if (reply == -2)
        {
            // conversation has been cancelled. kill it here rather than waiting for update. go back to start of convo
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            convNextPoint = convEntryPoint;
            // but change the hint text back to what it was pre-conversation
            hintText.text = hint;
            return;
        }

        // check the user hasn't selecetd 2 when there is only one reply
        print(reply);
        print(conversation.getNodes()[convPoint].getReplies().Length);
        if (reply+1 > conversation.getNodes()[convPoint].getReplies().Length)
            return;

        int replyIndex = conversation.getNodes()[convPoint].getReplyPointer()[reply];
        if (replyIndex == -1)
        {
            // conversation has ended. kill it here rather than waiting for update.
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            convNextPoint = convPoint;
            // but change the hint text back to what it was pre-conversation
            hintText.text = hint;
            return;
        }
        else if (isPickUp && replyIndex == pickUpIndex) {
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            convNextPoint = convPoint;
            GameState.GetActiveCharacter().GetComponent<Inventory>().Add(gameObject.name);
            if (!stayAfterPickUp) {
                gameObject.SetActive(false);
            }
        }
        else {
            //update the response so fixedUpdate will see the difference
            convNextPoint = replyIndex;
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


