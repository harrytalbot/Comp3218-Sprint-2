using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUnlock : MonoBehaviour
{

    public float rotationDegreesPerSecond = 45f;
    public float rotationDegreesAmount = 90f;

    private float totalRotation = 110;
    public bool openGate = false;

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {
        if (openGate)
            Open();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) & other.transform.parent.tag == "Player" & GameState.GetActiveCharacter().GetComponent<Inventory>().gateKey)
        {
            // open gate
            openGate = true;
            transform.Find("Lock").GetComponent<Rigidbody>().useGravity = true;
            transform.Find("Lock").GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 5f));

        }
    }

    void Open()
    {
        Quaternion newRotation = Quaternion.AngleAxis(-100, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);
    }
}