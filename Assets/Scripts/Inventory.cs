using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public bool gateKey;
    public bool otherStuff1;
    public bool otherStuff2;

    public void Add(string objectName) {

        if (objectName.Equals("Gate Key")) {
            gateKey = true;
        }
        else if (objectName.Equals("Other Stuff")) {
            otherStuff1 = true; // Placeholder
        }
        else
            Debug.Log("I don't recognise that item");
        GameState.UpdateIcon(this);
    }

    public bool GotItem(string objectName) {
        if (objectName.Equals("Gate Key")) {
            return gateKey;
        }
        else if (objectName.Equals("Other Stuff")) {
            return otherStuff1; // Placeholder
        }
        else {
            Debug.Log("I don't recognise that item");
            return false;
        }
    }
}
