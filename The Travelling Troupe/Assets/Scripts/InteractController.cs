using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour {

    public float activationDistance;
    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, activationDistance)) {
                Talkable tk = hit.collider.gameObject.GetComponent<Talkable>();
                if (tk != null) {
                    tk.interact();
                }                    
            }
        }
    }
}
