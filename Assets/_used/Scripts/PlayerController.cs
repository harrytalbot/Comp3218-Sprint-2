﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
    public Camera mainCam;
    private Talkable tk;
    public SphereCollider hintCollider;

    public float moveSpeed;
    public float rotationSpeed;
    public float jumpHeight;
    public float maxVelocityChange = 10.0f;
    public float activationDistance;
    public int characterNumber; // Donkey = 0, Cat = 1, Dog = 2, Chicken = 3    (These can be easily changed)

    // amount to increase raycast by
    public float yCast;

    private float gravity = 10;

    private bool isGrounded;
    
    void Awake() {
		rb = GetComponent<Rigidbody> ();
        hintCollider = GetComponentInChildren<SphereCollider>();

    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    
    private void OnCollisionStay(Collision collision) {
        if (!(isGrounded) && (collision.gameObject.tag == "Ground")) {
            OnCollisionEnter(collision);
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Ground") {
            isGrounded = false;
        }
    }

    void FixedUpdate() {
        if (GameState.activeCharacter == characterNumber && !GameState.isTalking)
        {
           // calculate rotation
           transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            // Calculate how fast we should be moving
            if (isGrounded) { 
            Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= moveSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            }
            if (Input.GetKey(KeyCode.Space) & isGrounded) {
                Jump();
            }

            // manual gravity?
            rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

            /*
            if (Input.GetKeyDown(KeyCode.E)) {

                //disable the sphere collider for use when checking hints, else the raycast will hit it
                hintCollider.gameObject.SetActive(false);
                
                RaycastHit hit;
                Debug.DrawRay(transform.position + new Vector3(0, yCast, 0), transform.forward*10, Color.green);
                if (Physics.Raycast(transform.position + new Vector3(0, yCast, 0), transform.forward*10, out hit, activationDistance)) {
                    tk = hit.collider.gameObject.GetComponent<Talkable>();                    
                    if (tk != null) {

                        tk.Interact();
                        GameState.isTalking = true;
                        GameState.conversationUI.SetActive(true);
                    }
                }

                //reenable the sphere collider for use when checking hints
                hintCollider.gameObject.SetActive(true);
            }
            */
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && (other.gameObject.GetComponent<Talkable>() != null) && GameState.activeCharacter == characterNumber)
        {
            other.gameObject.transform.LookAt(new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z));
            tk = other.gameObject.GetComponent<Talkable>();
            tk.Interact();
            GameState.isTalking = true;
            GameState.conversationUI.SetActive(true);
        }
    }

    void Jump() {
        // this will work like an arc
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), rb.velocity.z);
    }

    void Update() {


        if (GameState.activeCharacter != characterNumber){
            // if not active rotate to player
            transform.LookAt(new Vector3(GameState.GetActiveCharacter().transform.position.x, transform.position.y, GameState.GetActiveCharacter().transform.position.z));
        }

        //print("active character: " + GameState.activeCharacter);

        if (GameState.activeCharacter == characterNumber && GameState.isTalking) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                // pick option 1
               tk.setReply(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                // pick option 2
                tk.setReply(2);
            }
            else if (Input.GetKeyDown(KeyCode.E) && (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CatTalkAnim") || GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DonkeyTalkAnim"))) {
                // Cancel conversation
                print("e");
                tk.setReply(0);
            }
        }

    }
    
}
