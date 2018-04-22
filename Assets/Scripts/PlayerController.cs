using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
    public Camera mainCam;
    private Talkable tk;

    public float moveSpeed;
    public float rotationSpeed;
    public float jumpHeight;
    public float maxVelocityChange = 10.0f;
    public float activationDistance;
    public int characterNumber; // Donkey = 1, Dog = 2, Cat = 3, Chicken = 4    (These can be easily changed)

    private float gravity = 10;

    private bool isGrounded;
    
    void Awake() {
		rb = GetComponent<Rigidbody> ();
        mainCam = GetComponentInChildren<Camera>();
        characterNumber--;
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

            if (Input.GetKeyDown(KeyCode.E)) {
                RaycastHit hit;
                Debug.DrawRay(transform.position, transform.forward*10, Color.green);
                if (Physics.Raycast(transform.position, transform.forward*10, out hit, activationDistance)) {
                    tk = hit.collider.gameObject.GetComponent<Talkable>();                    
                    if (tk != null) {

                        // make player invisible - needs transparent shader
                        /*
                         * Material mat = transform.Find("Renderer").GetComponentInChildren<SkinnedMeshRenderer>().material;
                        Color newColor = mat.color;
                        newColor.a = 0.2f;
                        mat.color = newColor;
                        */


                        tk.Interact();
                        GameState.isTalking = true;
                        GameState.conversationUI.SetActive(true);
                    }
                }
            }
        }
    }


    void Jump() {
        // this will work like an arc
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), rb.velocity.z);
    }

    void Update() {

        if (GameState.isTalking && GameState.activeCharacter == characterNumber) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                // pick option 1
               tk.setReply(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                // pick option 2
                tk.setReply(1);
            }
            else if (Input.GetKeyDown(KeyCode.E) && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TalkAnim")) {
                // Cancel conversation
                tk.setReply(-2);
            }
        }

        /*
        if (!GameState.isTalking) {             
            // make player visible
            Material mat = transform.Find("Renderer").GetComponentInChildren<SkinnedMeshRenderer>().material;
            Color newColor = mat.color;
            newColor.a = 1;
            mat.color = newColor;
        }
        */
    }
    
}
