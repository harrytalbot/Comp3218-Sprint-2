using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation {
    
    public ConversationNode[] nodes;

    public ConversationNode[] getNodes()
    {
        return nodes;
    }

    [System.Serializable]
    public class ConversationNode
    {
        // the message
        public string message;        
        //the replies
        public string[] replies;
        // the replies index for this message
        public int[] repliesIndex;


        public string getMessage()
        {
            return message;
        }

        public int[] getReplyPointer()
        {
            return repliesIndex;
        }

        public string[] getReplies()
        {
            return replies;
        }
    }

}
