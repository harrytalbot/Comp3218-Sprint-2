using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public bool isPickup, stayAfterPickup;

	// Use this for initialization
	void Start () {
		
	}
	
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Q) && other.transform.parent.tag == "Player")
        {
                    print("asas)");

            if (isPickup)
            {
                GameState.GetActiveCharacter().GetComponent<Inventory>().Add(gameObject.name);

                if (!stayAfterPickup)
                {
                    gameObject.SetActive(false);
                }
            } else
                //just a mouse or something
                gameObject.SetActive(false);
        }
    }

}
