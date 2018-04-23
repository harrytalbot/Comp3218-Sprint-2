    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    // the current conversation point and the next one
    private int convPoint, convNextPoint;

    private StoryState storyState;

    public void Awake() {
        
        dialogueText = GameObject.Find("Dialogue").GetComponent<Text>();
        nameText = GameObject.Find("Name").GetComponent<Text>();
        // two buttons for responses
        buttonOne = GameObject.Find("ButtonOne");
        buttonTwo = GameObject.Find("ButtonTwo");
        buttonOneText = buttonOne.GetComponent<Button>().GetComponentInChildren<Text>();
        buttonTwoText = buttonTwo.GetComponent<Button>().GetComponentInChildren<Text>();
        hintText = GameObject.Find("Hint Text").GetComponent<Text>();

        storyState = GameObject.FindGameObjectWithTag("StoryState").GetComponent<StoryState>();

        dialoguePanel = GameObject.FindGameObjectWithTag("DialogueBox");
        hintPanel = GameObject.FindGameObjectWithTag("Hint");

        // find out where to start the conversation from
        //setStartPoint(storyState.GetStartPoint(talkableName));

    }

    void FixedUpdate() {
        // if these have changed, the user has selected an option
        if (convNextPoint != convPoint)
        {
            convPoint = convNextPoint;
            dialogueText.text = conversation.getNodes()[convNextPoint].getMessage();
            int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointers();
            string[] replies = conversation.getNodes()[convNextPoint].getReplies();
            buttonOneText.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
            if (replies.Length > 1)
            {
                buttonTwo.SetActive(true);
                buttonTwoText.GetComponentInChildren<Text>().text = (replies[1]);
                //buttonTwoText.GetComponentInChildren<Text>().text = (replies[1] + "(" + replyPointers[1] + ")");
            }
            else
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
        print(convNextPoint);
        dialogueText.text = conversation.getNodes()[convNextPoint].getMessage();
        nameText.text = talkableName;
        int[] replyPointers = conversation.getNodes()[convNextPoint].getReplyPointers();
        string[] replies = conversation.getNodes()[convNextPoint].getReplies();
        buttonOneText.GetComponentInChildren<Text>().text = (replies[0] + "(" + replyPointers[0] + ")");
        if (replies.Length > 1)
        {
            buttonTwo.SetActive(true);
            buttonTwoText.GetComponentInChildren<Text>().text = (replies[1]);
            //buttonTwoText.GetComponentInChildren<Text>().text = (replies[1] + "(" + replyPointers[1] + ")");
        }
        else
        {
            buttonTwo.SetActive(false);
        }
    }

    /** 
     * Method to convert the user's reply selection to the actual reply
     */
    public void setReply(int reply)
    {
        if (reply == 0)
        {
            print("e");
            // conversation has been cancelled. kill it here rather than waiting for update. don't change convo start point
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            convNextPoint = convEntryPoint;
            // but change the hint text back to what it was pre-conversation
            hintText.text = hint;
            return;
        }

        // subtract 1 to match array indexing
        reply--;

        // check the user hasn't selecetd 2 when there is only one reply
        if (reply+1 > conversation.getNodes()[convPoint].getReplies().Length) 
            return;

        // if the user has selected the second option and there is no reply index for it, this
        // acts as cancelling the conversation
        if (reply == 1 && conversation.getNodes()[convPoint].getReplyPointers().Length == 1)
        {
            // conversation has been cancelled. kill it here rather than waiting for update. don't change convo start point
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            convNextPoint = convEntryPoint;
            // but change the hint text back to what it was pre-conversation
            hintText.text = hint;
            return;
        }

        int replyIndex = conversation.getNodes()[convPoint].getReplyPointers()[reply];

        if (replyIndex < 0)
        {
            // less that 0 means a successful conversation at that level, e.g. returning -3 means 
            // the conversation finished successfully to the level 3 in storystate (started conv. 2)

            // conversation has ended. kill it here rather than waiting for update.
            GameState.isTalking = false;
            dialoguePanel.SetActive(false);
            // but change the hint text back to what it was pre-conversation
            hintText.text = hint;

            // tell the story state the conversation has finished so it keeps track
            convNextPoint = storyState.UpdateStoryProgress(talkableName, SceneManager.GetActiveScene().name, - replyIndex);
            convEntryPoint = convNextPoint;
            return;
        }
        else
        {
            //conversation still going . update the response so fixedUpdate will see the difference
            convNextPoint = replyIndex;
        }
    }

    public void setStartPoint(int startPoint)
    {
        convEntryPoint = startPoint;

        convNextPoint = startPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
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


