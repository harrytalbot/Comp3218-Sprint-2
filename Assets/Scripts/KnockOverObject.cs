using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class KnockOverObject : MonoBehaviour {

    public GameObject hintUI;
    public string hintUIText;
    public string wrongCharacterText;
    public string characterToUseThis = "Horse";
    public GameObject[] alertedNPCs;
    public Vector3[] alertedLocations;
    public float delay;

    private bool knockedOver = false;
    private bool sent = false;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !knockedOver) {
            hintUI.SetActive(true);
            GameObject.Find("Hint Text").GetComponent<Text>().text = hintUIText;
        }
    }

    private void OnTriggerExit(Collider other) {
        hintUI.SetActive(false);
    }

    private void OnTriggerStay(Collider other) {
        // if the other collider is the player, the user presses e and they have the key
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Player" && !knockedOver && other.gameObject.name.Contains(characterToUseThis)) {
            // Knock over
            knockedOver = true;
            if (gameObject.name.Contains("Rack"))
                anim.Play("KnockOver");
            else if (gameObject.name.Contains("Shelf"))
                anim.Play("ShelfKnockOver");
            else if (gameObject.name.Contains("Mirror"))
                anim.Play("MirrorKnockOver");
        }
        else if (Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Player" && !knockedOver && !other.gameObject.name.Contains(characterToUseThis))
            GameObject.Find("Hint Text").GetComponent<Text>().text = wrongCharacterText;
    }

    private void Update() {

        if (delay >= 0 && knockedOver) {
            delay -= Time.deltaTime;
        }
        else if (!sent && knockedOver) {
            for (int i = 0; i < alertedNPCs.Length; i++) {
                if (alertedNPCs[i] != null) {
                    alertedNPCs[i].GetComponentInChildren<EnemyVisionCone>().alerted = true;
                    alertedNPCs[i].GetComponentInChildren<EnemyVisionCone>().knocked = gameObject;
                    alertedNPCs[i].GetComponentInChildren<EnemyVisionCone>().material.color = Color.yellow;
                    alertedNPCs[i].GetComponent<NavMeshAgent>().SetDestination(alertedLocations[i]);
                }
            }
            sent = true;
            if (gameObject.name.Contains("Car")) {
                sent = false;
                knockedOver = false;
            }
        }
    }

}
