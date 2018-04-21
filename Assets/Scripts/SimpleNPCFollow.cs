using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPCFollow : MonoBehaviour {

    public GameObject player;
    private GameObject npc;
    private float targetDistance;
    public float allowedDistance = 5;
    public float followSpeed;
    private RaycastHit shot;

    void Start() {
        npc = GetComponent<GameObject>();
    }

    void Update () {
        transform.LookAt(player.transform);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot)) {
            targetDistance = shot.distance;
            if (targetDistance >= allowedDistance) {
                // Play move animation for NPC here
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed);
            }
            else {
                // Play idle animation for NPC here
            }
        }
	}
}
