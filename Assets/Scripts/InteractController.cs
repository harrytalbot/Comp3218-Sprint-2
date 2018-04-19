using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour {

    public float activationDistance;
    public GameObject myUI;

    void Update() {

        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, activationDistance)) {
                Talkable tk = hit.collider.gameObject.GetComponent<Talkable>();
                if (tk != null) {
                    tk.Interact();
                    if (!GameState.isTalking) {
                        GameState.isTalking = true;
                        myUI.SetActive(true);
                    }
                    else {
                        GameState.isTalking = false;
                        myUI.SetActive(false);
                    }
                }                    
            }
        }
    }
}
