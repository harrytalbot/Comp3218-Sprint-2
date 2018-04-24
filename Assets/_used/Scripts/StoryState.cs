using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryState : MonoBehaviour {

    // each thing that is talkable has a status defining how far into conversation(s) the player has got.
    int FarmerStatus, SheepStatus;
    int DonkeyStatus, CatStatus, TigerStatus, DuckStatus;

    int EndStatus;

    // this status comes from the return value of the convesation when it's less that 0, e.g. returning -3 means 
    // the conversation finished successfully to the level 3 in storystate

    // also keeps track of the conversation start point, as it will decide where conversations should be started from
    int FarmerStart, SheepStart;
    int DonkeyStart, CatStart, TigerStart, DuckStart;

    int EndStart;

    // keep track of where the start points should be for each level as an array, find the right start point given a level as FarmerStarts[FarmerStatus]
    int[] FarmerStarts, SheepStarts;
    int[] DonkeyStarts, CatStarts, TigerStarts, DuckStarts;

    int[] EndStarts;

    // Talkable references
    Talkable tkFarmer, tkSheep;
    Talkable tkDonkey, tkCat, tkTiger, tkDuck;

    Talkable tkEnd;

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public int miceToCatch;
    public int miceCaught = 0;

    public bool hasCat, hasTiger;

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
            TigerStarts = new int[] { 0, 5, 6, 8, 10, 15, 16 }; //0 = first, 5 = hasn't got water, 6 = has got water, 8 = asking again, 10 = after given water, 15 = is coming to city, 
            DuckStarts = new int[] {0, 4, 8, 12, 13, 14}; // 0 = no companions, 4 = tiger or cat, 8 = both, 12 is asking about more people, 13 = has enough, 14 = coming to city

            DonkeyStatus = 0; DonkeyStart = DonkeyStarts[DonkeyStatus];
            CatStatus = 0; CatStart = CatStarts[CatStatus];
            TigerStatus = 0; TigerStart = TigerStarts[TigerStatus];
            DuckStatus = 0; DuckStart = DuckStarts[DuckStatus];

            tkDonkey = GameObject.FindGameObjectWithTag("Donkey").transform.parent.GetComponent<Talkable>();
            tkDonkey.setStartPoint(DonkeyStarts[DonkeyStatus]);

            tkCat = GameObject.FindGameObjectWithTag("Cat").transform.parent.GetComponent<Talkable>();
            tkCat.setStartPoint(CatStarts[CatStatus]);

            tkTiger = GameObject.FindGameObjectWithTag("Tiger").transform.parent.GetComponent<Talkable>();
            tkTiger.setStartPoint(TigerStarts[TigerStatus]);

            tkDuck = GameObject.FindGameObjectWithTag("Duck").transform.parent.GetComponent<Talkable>();
            tkDuck.setStartPoint(DuckStarts[DuckStatus]);

            setupMainConversations();

            hasCat = false; hasTiger = false;

        }
        else if (SceneManager.GetActiveScene().name == "House Level")
        {


        }
        else if (SceneManager.GetActiveScene().name == "End Level")
        {
            EndStarts = new int[] { 0 };

            EndStatus = 0; EndStart = EndStarts[EndStatus];

            tkEnd = GameObject.FindGameObjectWithTag("End").transform.parent.GetComponent<Talkable>();
            tkEnd.setStartPoint(EndStarts[EndStatus]);

            setupEndConversations();
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
                        print("enough");
                        CatStatus = 2;
                    }
                }

                if (CatStatus == 3)
                {
                    GameState.UnlockCharacter(tkCat.transform.GetComponent<PlayerController>().characterNumber);
                    hasCat = true;
                    if (hasCat && hasTiger && DuckStatus == 0)
                    {
                        DuckStatus = 2;
                        tkDuck.setStartPoint(DuckStarts[DuckStatus]);
                    }
                    else if (hasCat && hasTiger && DuckStatus == 3)
                    {
                        DuckStatus = 4;
                        tkDuck.setStartPoint(DuckStarts[DuckStatus]);
                    }
                }


                return CatStarts[CatStatus];
            }

            else if (talkableName == "Donkey")
            {
                DonkeyStatus = level;

                return DonkeyStarts[DonkeyStatus];
            }

            else if (talkableName == "Tiger")
            {
                TigerStatus = level;

                if (TigerStatus == 5)
                {
                    GameState.UnlockCharacter(tkTiger.transform.GetComponent<PlayerController>().characterNumber);
                    hasTiger = true;
                    if (hasCat && hasTiger && DuckStatus == 0)
                    {
                        DuckStatus = 2;
                        tkDuck.setStartPoint(DuckStarts[DuckStatus]);
                    }
                    else if (hasCat && hasTiger && DuckStatus == 3)
                    {
                        DuckStatus = 4;
                        tkDuck.setStartPoint(DuckStarts[DuckStatus]);
                    }
                }

                return TigerStarts[TigerStatus];
            }
            else if (talkableName == "Duck")
            {
                DuckStatus = level;

                if (DuckStatus == 5)
                {
                    GameState.UnlockCharacter(tkDuck.transform.GetComponent<PlayerController>().characterNumber);
                }
                    return DuckStarts[DuckStatus];
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

                tkCat.setStartPoint(CatStarts[CatStatus]);

            }

        }
        else if (itemName == "Bucket")
        {
            print("water");
            TigerStatus = 2;
            tkTiger.setStartPoint(TigerStarts[TigerStatus]);
        }
    }

    void setupFarmConversations()
    {
        Conversation farmerConvo = new Conversation();

        string[] farmerMessages = new string[] {
            "Donkey, I'm afraid I have some bad news to tell you. I've been out on the fields today and the crop is looking terrible. I don't know what's gone wrong, and it's nearly time for harvest which only worries me more.",
            "Well that's the bad news. It's not what we can do, it's what I can do. You're old and tired, and not much use in the fields any more...",
            "I think you mean you spent most of the summer lying around doing nothing, you lazy mule. I needed your help and now... I'm going to have to sell you.",
            "There's a farm over the Hill that has offered me good money for your, considering your age. I have bills due and so you'll be leaving tomorrow.",
            "That's not what I meant... I'm going to have to sell you. I need the money fast. ",
            "Unfortunately it's my only option.. All the other livestock will make me money over time - the sheep, chickens and cows. You just cost me more and more",
            "I know, and I hope you understand this wasn't an easy decision for me to make. Now, I need to go and repair my car...",
            " ",
            "Have you seen my Keys anywhere? they've been missing for days now..."
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
            "I couldn't help but overhear the Boss over there... you alright?",
            "Not like there's a lot to do here either. I've heard the big city is the place to be. Bars, clubs, so much to do... and you can be anything! All the big music stars are bred in the city!",
            "Ha! I'm not sure if you've heard but that might not be neccesary...",
            "Well... You know the farmer crashed his car the other day, into the lake? I heard him mumbling to himself he lost his keys nearby and he can't find them.",
            "So the Key might be near the pond! I've got no interest in leaving but If you do now might be the best chance you'll get. Good Luck!",
            " ",
            "You got the Key?! Try it on the lock and get out of here!",
            " ",
            "I think the Farmer was looking for you.",
            "He was be over by the farmhoouse last time I saw him.",
            " ",
            "The Key to the gate might be near the pond, have a look!"
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
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Conversation catConvo = new Conversation();

        string[] catMessages = new string[] {
            "Ah, another animal, out on the road to the big city.",
            "Certainly, just follow the beaten track.",
            "I've been cast out by my owner. While I may be a big cat, I'm no use to him any more, and just wish to lie by the fire.",
            "I don't know if I will be any help to you either, I'm old and slow. I might just stay here and try to catch these mice.",
            "Hmm... Well, If I suppose if you catch enough, I'll come along with you, seeing as it will take me an age to get them alone.",
            " ",
            "How's it going?", // not enough
            "How's it going?", // has enough
            "Seems like enough! Let's go.",
            "Hmm, There's still a lot to catch. I'm going to stay here.",
            " ",
            "I think I'll make a good musician.",
            "I'll catch all these mice.. one day..."
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Conversation tigerConvo = new Conversation();

        string[] tigerMessages = new string[] {
            "Need... Water... So Thirsty in this heat....",
            "I can't handle the heat... I've been shunned from my family, and moved here from Asia, but it's still too hot.",
            "I'm so lost, and dehydrated, I've no idea where to get one.",
            "Do You?! Please, bring it to me, I'll forever be in your debt.",
            " ",
            "Have you got any water?", // hasn't got water
            "Have you got any water?", //has got water
            " ",
            "Have you found some water anywhere?",
            " ",
            "Sir, I was so dehydrated, I owe you my life. However can I repay you?",
            "With pleasure! Back home I was a Drummer, and longed of being famous in a band!",
            "Ummm.... you have no idea what you're doing?",
            "  ",
            "After my near death experience I don't think I can chance that. Best of luck with your travels, I'll be staying here.",
            "Onwards, lead the way to the city lights!",
            "I'm sure I'll get used to the heat eventually... eventually... ",
        };

        tigerConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(tigerMessages[0], new int[] { 1, 2}, new string[] { "How are you thirsty? It's barely 20C, and you're a tiger.", "Don't you know where to grab a drink?" }),
            new Conversation.ConversationNode(tigerMessages[1], new int[] { 2 }, new string[] { "Surely there must be somewhere round here you can get a drink?", "Thanks, I'll be on my way then!" }),
            new Conversation.ConversationNode(tigerMessages[2], new int[] { 3 }, new string[] { "I know where I can get some water..", "I'm afraid I'm new round here too. I've no idea where to go."}),
            new Conversation.ConversationNode(tigerMessages[3], new int[] { -1 }, new string[] { "Will do."}),
            new Conversation.ConversationNode(tigerMessages[4], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(tigerMessages[5], new int[] { -3}, new string[] { "I'll be honest, I don't know where to find any.", "Not yet!"}),
            new Conversation.ConversationNode(tigerMessages[6], new int[] {10 }, new string[] { "Here it is!"}),
            new Conversation.ConversationNode(tigerMessages[7], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(tigerMessages[8], new int[] {-1 }, new string[] { "Yes, I'll go and get some now!", "No, Sorry!"}),
            new Conversation.ConversationNode(tigerMessages[9], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(tigerMessages[10], new int[] {11, 12 }, new string[] {"Well, I'm going to The city to become a musician. Would you like to come along?", "I've left everything I know to go to the city with no real idea what I'm doing. Wanna come?"}),
            new Conversation.ConversationNode(tigerMessages[11], new int[] {-5 }, new string[] { "Follow me!"}),
            new Conversation.ConversationNode(tigerMessages[12], new int[] {11, 14 }, new string[] { "Well, The plan really is to become musicians. Come with us!", "Hardly. I am just a Donkey, after all."}),
            new Conversation.ConversationNode(tigerMessages[13], new int[] { }, new string[] { }),
            new Conversation.ConversationNode(tigerMessages[14], new int[] {-6 }, new string[] { "Suit yourself. Anway, see ya!"}),
            new Conversation.ConversationNode(tigerMessages[15], new int[] {-5 }, new string[] { "This way!"}),
            new Conversation.ConversationNode(tigerMessages[16], new int[] { -6 }, new string[] { "I'll remember you when I'm famous!" }),


        };

        tkTiger.conversation = tigerConvo;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Conversation duckConvo = new Conversation();

        string[] duckMessages = new string[] {
            "You don't look like you're from round here. Can I help you?",
            "I see. Where are you from? We don't get many outsiders from across the pond.",
            "The City! I'd love to go there, but no one would ever go with me, and I'd like to stay in a group of four at least.",
            " ",
            "You don't look like you're from round here. Can I help you?",
            "I see. Where are you from? We don't get many outsiders from across the pond.",
            "The City! I'd love to go there, but I'd rather go in a bigger group... is anyone else going with you?",
            " ",
            "You don't look like you're from round here. Can I help you?",
            "I see. Where are you from? We don't get many outsiders from across the pond.",
            "The City! I'd love to go there! Can I come with you?",
            " ",
            "Have you two companions yet?",
            "We'd made a group of four, can I come with you?",
            "I can't wait to get to the city. I bet they have a huge park, full of lakes and other ducks..."
        };

        duckConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(duckMessages[0], new int[] { 1 }, new string[] { "You're pretty well spoken for a duck. Im actually just passing through.", "I think I'm fine, actually." }),
            new Conversation.ConversationNode(duckMessages[1], new int[] { 2 }, new string[] { "Just a small Farm, down the road. I've left and am heading to the City."}),
            new Conversation.ConversationNode(duckMessages[2], new int[] { -3 }, new string[] { "Well, If I find more travellers I'll come back to you.", }),
            new Conversation.ConversationNode(duckMessages[3], new int[] {   }, new string[] {}),
            new Conversation.ConversationNode(duckMessages[4], new int[] { 5 }, new string[] { "You're pretty well spoken for a duck. Im actually just passing through.", "I think I'm fine, actually." }),
            new Conversation.ConversationNode(duckMessages[5], new int[] { 6 }, new string[] { "Just a small Farm, down the road. I've left and am heading to the City."}),
            new Conversation.ConversationNode(duckMessages[6], new int[] { -3 }, new string[] { "Not at the moment, but if anyone else joins I'll be sure to find you!", }),
            new Conversation.ConversationNode(duckMessages[7], new int[] {   }, new string[] {}),
            new Conversation.ConversationNode(duckMessages[8], new int[] { 9 }, new string[] { "You're pretty well spoken for a duck. Im actually just passing through.", "I think I'm fine, actually." }),
            new Conversation.ConversationNode(duckMessages[9], new int[] { 10 }, new string[] { "Just a small Farm, down the road. I've left and am heading to the City."}),
            new Conversation.ConversationNode(duckMessages[10], new int[] { -5 }, new string[] { "Of course! Follow me.", }),
            new Conversation.ConversationNode(duckMessages[11], new int[] {   }, new string[] {}),
            new Conversation.ConversationNode(duckMessages[12], new int[] {-3}, new string[] {"Nope, not yet!"}),
            new Conversation.ConversationNode(duckMessages[13], new int[] { -5 }, new string[] { "Of course! Follow me.", "Sorry, four might be too many."}),
            new Conversation.ConversationNode(duckMessages[14], new int[] {-5}, new string[] {"I'm not sure you understand what a city is, but ok!"}),



        };

        tkDuck.conversation = duckConvo;


    }

    void setupEndConversations()
    {
        Conversation endConvo = new Conversation();

        string[] endMessages = new string[] {
            " END MESSAGE TEXT GOES HERE",
        };

        endConvo.nodes = new Conversation.ConversationNode[]{
            new Conversation.ConversationNode(endMessages[0], new int[] { -1 }, new string[] { "That Farm has driven you crazy..." }),
        };

        tkDonkey.conversation = endConvo;
    }

}
