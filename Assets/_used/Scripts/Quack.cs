using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quack : MonoBehaviour {

    private GameObject[] enemies;

    public float range = 10f;
    public float distractLength = 5;

    private void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            foreach (GameObject current in enemies) {
                if (Vector3.Distance(transform.position, current.transform.position) < range) {
                    if (!current.GetComponentInChildren<EnemyVisionCone>().alerted) {
                        current.GetComponentInChildren<EnemyVisionCone>().GetDistracted(transform, distractLength);
                    }
                }
            }
        }
    }
}
