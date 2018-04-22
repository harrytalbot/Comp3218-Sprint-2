using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionCone : MonoBehaviour {

    public float coneAngle;
    public float coneDistance;
    private GameObject[] targets;

	// Use this for initialization
	void Start () {
        targets = GameState.GetCharacters();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
