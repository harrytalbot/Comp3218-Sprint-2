using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryState : MonoBehaviour {

    // each thing that is talkable has a status defining how far into conversation(s) the player has got.
    int FarmerStatus, SheepStatus, CatStatus;

    // this status comes from the return value of the convesation when it's less that 0, e.g. returning -3 means 
    // the conversation finished successfully to the level 3 in storystate (started conv. 2)

    // also keeps track of the conversation start point, as it will decide where conversations should be started from
    int FarmerStart, SheepStart, CatStart;

    // keep track of where the start points should be for each level as an array
    int[] FarmerStarts = new int[] { 0, 8 }; // 0 = about leaving, 8 = have you seen my keys
    int[] SheepStarts = new int[] { 0, 6 }; // 0 = about hearing conversation with farmer, 6 = bleats
    int[] CatStarts = new int[] { 0, 3 };

    // and find the right start point given a level as FarmerStarts[FarmerStatus]

    // Talkable references
    Talkable tkFarmer, tkSheep, tkCat;

	// Use this for initialization
	void Start () {
        FarmerStatus = 0;   FarmerStart = FarmerStarts[FarmerStatus];
        SheepStatus = 1;    SheepStart = SheepStarts[SheepStatus];
        CatStatus = 0;      CatStart = CatStarts[CatStatus];

        tkFarmer = GameObject.FindGameObjectWithTag("Farmer").transform.parent.GetComponent<Talkable>();
        tkFarmer.setStartPoint(FarmerStarts[FarmerStatus]);
        tkSheep = GameObject.FindGameObjectWithTag("Sheep").transform.parent.GetComponent<Talkable>();
        tkSheep.setStartPoint(SheepStarts[SheepStatus]);

        //tkCat = GameObject.FindGameObjectWithTag("Cat").transform.parent.GetComponent<Talkable>();

    }

    // whenever a successful conversation finishes, this method is called and a new
    // (or possibly the same, if the convo has been done twice) start point is returned
    public int UpdateStoryProgress(string talkableName, int level)
    {
        if (talkableName == "Farmer")
        {
                FarmerStatus = level;
                if (FarmerStatus == 1)
                {
                    // had conversation with farmer. sheep will say about hearing it
                    SheepStart = 0;
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
                // keep bleating
                return SheepStarts[SheepStatus];
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

            // sheep says about escaping (TODO)
            //SheepStatus = 2;
            //tkSheep.setStartPoint(SheepStarts[SheepStatus]);
        }
    }
}
