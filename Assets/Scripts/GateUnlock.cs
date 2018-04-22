using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GateUnlock : MonoBehaviour
{

    public float rotationDegreesPerSecond = 45f;
    public float rotationDegreesAmount = 90f;

    private float totalRotation = 110;
    public bool openGate = false;

    public GameObject hintUI;
    public string hintUIText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hintUI.SetActive(true);
            GameObject.Find("Hint Text").GetComponent<Text>().text = hintUIText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hintUI.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
<<<<<<< HEAD
        // if the other collider is the player, the user presses e and they have the key
=======
>>>>>>> 68b29e09814371b54d8cc8e61fe97b1f9d0ac33e
        if (Input.GetKeyDown(KeyCode.E) && other.transform.parent.tag == "Player" && GameState.GetActiveCharacter().GetComponent<Inventory>().gateKey)
        {
            // open gate
            openGate = true;
            transform.Find("Lock").GetComponent<Rigidbody>().useGravity = true;
            transform.Find("Lock").GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 5f));

        }
    }

    void Update()
    {
        if (openGate)
            Open();
    }

    void Open()
    {
        Quaternion newRotation = Quaternion.AngleAxis(-100, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);
    }
}