using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scare : MonoBehaviour {

    private GameObject[] enemies;

    private void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            foreach (GameObject current in enemies) {
                if (Vector3.Distance(transform.position, current.transform.position) < 10) {
                    if (current.GetComponentInChildren<EnemyVisionCone>().alerted) {
                        current.GetComponentInChildren<EnemyVisionCone>().GetScared();
                    }
                }
            }
        }
    }
}
