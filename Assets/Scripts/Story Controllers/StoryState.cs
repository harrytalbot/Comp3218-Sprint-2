using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryState : MonoBehaviour {

    // each thing that is talkable has a status defining how far into conversation(s) the player has got.
    static int FarmerStatus, SheepStatus;
    static int DonkeyStatus, CatStatus, DogStatus;

    // this status comes from the return value of the convesation when it's less that 0, e.g. returning -3 means 
    // the conversation finished successfully to the level 3 in storystate

    // also keeps track of the conversation start point, as it will decide where conversations should be started from
    static int FarmerStart, SheepStart;
    static int DonkeyStart, CatStart, DogStart;

    // keep track of where the start points should be for each level as an array, find the right start point given a level as FarmerStarts[FarmerStatus]
    int[] FarmerStarts, SheepStarts;
    int[] DonkeyStarts, CatStarts, DogStarts;

    // Talkable references
    Talkable tkFarmer, tkSheep;
    Talkable tkDonkey, tkCat, tkDog;

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public int miceToCatch;
    private int miceCaught = 0;


	// Use this for initialization
	void Start () {

        // if farm level
        if (SceneManager.GetActiveScene().name == "Farm Level")
        {
            print(SceneManager.GetActiveScene().name);

            FarmerStarts = new int[] { 0, 8 }; // 0 = about leaving, 8 = have you seen my keys
            SheepStarts = new int[] { 0, 6, 8, 11 }; // 0 = about hearing conversation with farmer, 6 = has key, 8 = farmer loking for you, 11 = key is by pond

            // setup for farm
            FarmerStatus = 0; FarmerStart = FarmerStarts[FarmerStatus];
            SheepStatus = 2; SheepStart = SheepStarts[SheepStatus];

            tkFarmer = GameObject.FindGameObjectWithTag("Farmer").transform.parent.GetComponent<Talkable>();
            tkFarmer.setStartPoint(FarmerStarts[FarmerStatus]);
            tkSheep = GameObject.FindGameObjectWithTag("Sheep").transform.parent.GetComponent<Talkable>();
            tkSheep.setStartPoint(SheepStarts[SheepStatus]);

            setupFarmConversations();
        }
           // if main level
        else if (SceneManager.GetActiveScene().name == "Main Level")
        {
            DonkeyStarts = new int[] { 0 };
            CatStarts = new int[] { 0, 6, 7, 11, 12 }; // 0 = first conversation, 6 = has said will catch mice but hasn't caught enough, 7 = has catch enough, 11 = convo when in team, 12 = convo when staying put
            DogStarts = new int[] { 0 };

            DonkeyStatus = 0; DonkeyStart = DonkeyStarts[DonkeyStatus];
            CatStatus = 0; CatStart = CatStarts[CatStatus];
            DogStatus = 0; DogStart = DogStarts[DogStatus];

            tkDonkey = GameObject.FindGameObjectWithTag("Donkey").transform.parent.GetComponent<Talkable>();
            tkDonkey.setStartPoint(DonkeyStarts[DonkeyStatus]);
            tkCat = GameObject.FindGameObjectWithTag("Cat").transform.parent.GetComponent<Talkable>();
            tkCat.setStartPoint(CatStarts[CatStatus]);

            // tkDog = GameObject.FindGameObjectWithTag("Dog").transform.parent.GetComponent<Talkable>();
            // tkDog.setStartPoint(DogStarts[DogStatus]);

            setupMainConversations();

        }
        else if (SceneManager.GetActiveScene().name == "House Level")
        {


        }

    }

    // whenever a successful conversation finishes, this method is called and a new
    // (or possibly the same, if the convo has been done twice) start point is returned
    public int UpdateStoryProgress(string talkableName, string sceneName, int level)
    {
        if (sceneName == "Farm Level")
        {
            if (talkableName == "Farmer")
            {
                FarmerStatus = level;
                if (FarmerStatus == 1)
                {
                    // had conversation with farmer. sheep will say about hearing it
                    SheepStatus = 0;
                    tkSheep.setStartPoint(SheepStarts[SheepStatus]);
                }

                // farmer will now just say about loosing keys
                return FarmerStarts[FarmerStatus];
            }

            if (talkableName == "Sheep")
            {
                SheepStatus = level;
                if (SheepStatus == 1)
                {
                    // keep saying about leaving
                    return SheepStarts[SheepStatus];
                }

                if (SheepStatus == 2)
                {
                    // keep saying about farmer looking
                    return SheepStarts[SheepStatus];
                }

                if (SheepStatus == 3)
                {
                    // keep saying about farmer looking
                    return SheepStarts[SheepStatus];
                }
            }
        }
        else if (sceneName == "Main Level")
        {
            if (talkableName == "Cat")
            {
                CatStatus = level;

                // if the player has spoken to cat about catching mice, check if there are enough so the right convo can be set
                if (CatStatus > 0 && CatStatus < 3)
                {
                    // player has stopped catching. check they have enough
                    if (miceCaught < miceToCatch)
                    {
                        CatStatus = 1;
                    }
                    else
                    {
                        CatStatus = 2;
                    }
                }

                return CatStarts[CatStatus];
            }
        }

        return 0;          
    }

    public int GetStartPoint(string talkableName)
    {
        if (talkableName == "Farmer")
            return FarmerStarts[FarmerStatus];
        else if (talkableName == "Sheep")
            return SheepStarts[SheepStatus];
        else return 0;
        
    }

    public void collected(string itemName)
    {
        if (itemName == "Gate Key")
        {
            // make farmer say about loosing his keys, no mention of leaving
            FarmerStatus = 1;
            tkFarmer.setStartPoint(FarmerStarts[FarmerStatus]);

            // sheep says about escaping
            SheepStatus = 1;
            tkSheep.setStartPoint(SheepStarts[SheepStatus]);
        }
        else if (itemName == "Mouse")
        {
            miceCaught++;

            // if the player has spoken to cat about catching mice, check if there are enough so the right convo can be set
            if (CatStatus > 0)
            {
                // player has stopped catching. check they have enough
                if (miceCaught < miceToCatch)
                {
                    CatStatus = 1;
                }
                else
                {
                    CatStatus = 2;
                }
            }

        }
    }

    void setupFarmConversations()
    {
        Conversation farmerConvo = new Conversation();

        string[] farmerMessages = new string[] {
            "0 Donkey, I'm afraid I have some bad news to tell you. I've been out on the fields today and the crop is looking terrible. I don't know what's gone wrong, and it's nearly time for harvest which only worries me more.",
            "1 Well that's the bad news. It's not what we can do, it's what I can do. You're old and tired, and not much use in the fields any more...",
            "2 I think you mean you spent most of the summer lying around doing nothing, you lazy mule. I needed your help and now... I'm going to have to sell you.",
            "3 There's a farm over the Hill that has offered me good money for your, considering your age. I have bills due and so you'll be leaving tomorrow.",
            "4 That's not what I meant... I'm going to have to sell you. I need the money fast. ",
            "5 Unfortunately it's my only option.. All the other livestock will make me money over time - the sheep, chickens and cows. You just cost me more and more",
            "6 I know, and I hope you understand this wasn't an easy decision for me to make. Now, I need to go and repair my car...",
            " ",
            "7 Have you seen my Keys anywhere? they've been missing for days now..."
        };

        farmerConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(farmerMessages[0], new int[] { 1, 2 }, new string[] { "What can we do? Surely it's not too late to get out there and put in extra work.", "Well, you did spend most of the summer lying around in the sun, doing nothing." }),
            new Conversation.ConversationNode(farmerMessages[1], new int[] { 3, 4 }, new string[] { "What are you trying to say?", "About time I retired!" }),
            new Conversation.ConversationNode(farmerMessages[2], new int[] { 5 }, new string[] { "Selling?! Surely there's no need for that!" }),
            new Conversation.ConversationNode(farmerMessages[3], new int[] { 5 }, new string[] { "So just like that, you're getting rid of me?" }),
            new Conversation.ConversationNode(farmerMessages[4], new int[] { 5 }, new string[] { "So just like that, you're getting rid of me?" }),
            new Conversation.ConversationNode(farmerMessages[5], new int[] { 6, -1 }, new string[] { "I'd make more money for you if I wasn't so old!", "I'd make money for you if I cared about this farm more!" }),
            new Conversation.ConversationNode(farmerMessages[6], new int[] { -1 }, new string[] { "I better go and say my goodbyes to the others then." }),
            new Conversation.ConversationNode(farmerMessages[7], new int[] {  }, new string[] {  }),
            new Conversation.ConversationNode(farmerMessages[8], new int[] { -1 }, new string[] { "Nope, Sorry!" }),
        };

        tkFarmer.conversation = farmerConvo;
        
        Conversation sheepConvo = new Conversation();

        string[] sheepMessages = new string[] {
            "0 I couldn't help but overhear the Boss over there... you alright?",
            "1 Not like there's a lot to do here either. I've heard the big city is the place to be. Bars, clubs, so much to do... and you can be anything! All the big music stars are bred in the city!",
            "2 Ha! I'm not sure if you've heard but that might not be neccesary...",
            "3 Well... You know the farmer crashed his car the other day, into the lake? I heard him mumbling to himself he lost his keys nearby and he can't find them.",
            "4 So the Key might be near the pond! I've got no interest in leaving but If you do now might be the best chance you'll get. Good Luck!",
            " ",
            "5 You got the Key?! Try it on the lock and get out of here!",
            " ",
            "I think the Farmer was looking for you.",
            "9 He was be over by the farmhoouse last time I saw him.",
            " ",
            "11 The Key to the gate might be near the pond, have a look!"
        };

        sheepConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(sheepMessages[0], new int[] { 2, 1 }, new string[] { "Yeah, but my days here are numbered. Better enjoy them while I can.", "Years of work and this is all it comes to. If I wasn't so old I'd jump the fence." }),
            new Conversation.ConversationNode(sheepMessages[1], new int[] { 3 }, new string[] { "Not that I'd be able to get out with the lock on that gate. The Farmer is the only one with the Key!", "Don't be ridiculous. I wouldn't even stand a chance at getting out." }),
            new Conversation.ConversationNode(sheepMessages[2], new int[] { 3, 3 }, new string[] { "What do you mean?", "Go on..." }),
            new Conversation.ConversationNode(sheepMessages[3], new int[] { 4, 4 }, new string[] { "Which means...", "I'm not sure I follow." }),
            new Conversation.ConversationNode(sheepMessages[4], new int[] { -3 }, new string[] { "Thanks Sheep!" }),
            new Conversation.ConversationNode(sheepMessages[5], new int[] {  }, new string[] {  }),
            new Conversation.ConversationNode(sheepMessages[6], new int[] { -1 }, new string[] { "On it!" }),
            new Conversation.ConversationNode(sheepMessages[7], new int[] {  }, new string[] {  }),
            new Conversation.ConversationNode(sheepMessages[8], new int[] { 9 }, new string[] { "Where is he?", "He can wait, I'll find him later" }),
            new Conversation.ConversationNode(sheepMessages[9], new int[] { -3 }, new string[] { "Thanks!" }),
            new Conversation.ConversationNode(sheepMessages[10], new int[] {  }, new string[] {  }),
            new Conversation.ConversationNode(sheepMessages[11], new int[] { -3 }, new string[] { "Thanks!" }),
        };

        tkSheep.conversation = sheepConvo;


    }

    void setupMainConversations()
    {
        Conversation donkeyConvo = new Conversation();

        string[] donkeyMessages = new string[] {
            "0 Donkey Donkey Donkey Donkey Donkey Donkey. ",
        };

        donkeyConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(donkeyMessages[0], new int[] { -1 }, new string[] { "That Farm has driven you crazy..." }),
        };

        tkDonkey.conversation = donkeyConvo;

        Conversation catConvo = new Conversation();


        string[] catMessages = new string[] {
            "0 Ah, another animal, out on the road to the big city.",
            "1 Certainly, just follow the beaten track.",
            "2 I've been cast out by my owner. While I may be a big cat, I'm no use to him any more, and just wish to lie by the fire.",
            "3 I don't know if I will be any help to you either, I'm old and slow. I might just stay here and try to catch these mice.",
            "4 Hmm... Well, If I suppose if you catch enough, I'll come along with you, seeing as it will take me an age to get them alone.",
            " ",
            "6 How's it going?", // not enough
            "7 How's it going?", // has enough
            "8 Seems like enough! Let's go.",
            "9 Hmm, There's still a lot to catch. I'm going to stay here.",
            " ",
            "11 I think I'd made a good musician.",
            "12 I'll catch all these mice.. one day..."
        };

        catConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(catMessages[0], new int[] { 1, 2}, new string[] { "Am I headed the right way?", "What are you doing here?" }),
            new Conversation.ConversationNode(catMessages[1], new int[] { 2 }, new string[] { "What are you doing here?", "Thanks, I'll be on my way then!" }),
            new Conversation.ConversationNode(catMessages[2], new int[] { 3 }, new string[] { "Me too. Why not come with me? Im going to the city to become a musician"}),
            new Conversation.ConversationNode(catMessages[3], new int[] { 4 }, new string[] { "What if I catch them for you?", "Alright, well good luck!" }),
            new Conversation.ConversationNode(catMessages[4], new int[] { -1 }, new string[] { "Alright, won't be long!", "Actually, I don't want too. Gotta go!" }),
            new Conversation.ConversationNode(catMessages[5], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(catMessages[6], new int[] { 9 }, new string[] {"Pretty good, I think that's enough!", "I'm just going to get some more!"}), // when they don't have enough
            new Conversation.ConversationNode(catMessages[7], new int[] { 8 }, new string[] {"Pretty good, I think that's enough!", "I'm just going to get some more!"}), //when they have enough
            new Conversation.ConversationNode(catMessages[8], new int[] { -3 }, new string[] {"Great!"}),
            new Conversation.ConversationNode(catMessages[9], new int[] { -4 }, new string[] { "Ok, good luck!"}),
            new Conversation.ConversationNode(catMessages[10], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(catMessages[11], new int[] { -3 }, new string[] { "Our Band will be the best in the city."}),
            new Conversation.ConversationNode(catMessages[12], new int[] { -4 }, new string[] { "I think you'll need to actually move if you want to catch them." }),

        };

        tkCat.conversation = catConvo;


    }
}
