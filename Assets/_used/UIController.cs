using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("DialogueBox").SetActive(false);
    }
}
