using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavFollowPlayer : MonoBehaviour {

    private GameObject player;
    private GameObject npcObject;
    private NavMeshAgent npc;
    private bool reset = false;

    private int characterNumber;


	// Use this for initialization
	void Start () {
        npc = GetComponent<NavMeshAgent>();
        npcObject = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameState.IsUnlocked(npcObject)) {
            player = GameState.GetActiveCharacter();

            if (!player.Equals(npcObject) && reset) {
                GetComponent<NavMeshAgent>().enabled = true;
                reset = false;
                npc.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                if (Vector3.Distance(transform.position, player.transform.position) > 5) {
                    npc.destination = player.transform.position;
                }
                else if (Vector3.Distance(transform.position, player.transform.position) <= 5)
                    npc.destination = transform.position;
            }
            else if (!player.Equals(npcObject)) {
                if (Vector3.Distance(transform.position, player.transform.position) > 5) {
                    npc.destination = player.transform.position;
                }
                else if (Vector3.Distance(transform.position, player.transform.position) <= 5)
                    npc.destination = transform.position;
            }
            else if (player.Equals(npcObject) && GetComponent<NavMeshAgent>().enabled) {
                GetComponent<NavMeshAgent>().enabled = false;
                reset = true;
            }
        }
        else
            npc.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}
}
