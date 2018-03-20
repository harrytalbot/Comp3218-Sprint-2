using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
    public Camera mainCam;

	public float speed;

    private bool isGrounded;
	private bool isPlatformed;

    public Vector3 checkpoint = new Vector3(0f, 0f, 0f);

    void Awake() {
		rb = GetComponent<Rigidbody> ();
		Cursor.lockState = CursorLockMode.Locked;
	}

    void OnCollisionEnter(Collision other) {
        /*if (other.gameObject.tag == "Platform") {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
                if (hit.collider.gameObject.Equals(other.gameObject)) {
                    isPlatformed = true;
                    return;
                }
            }
            if (Physics.Raycast(new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z), -Vector3.up, out hit)) {
                if (hit.collider.gameObject.Equals(other.gameObject)) {
                    isPlatformed = true;
                    return;
                }
            }
            if (Physics.Raycast(new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z), -Vector3.up, out hit)) {
                if (hit.collider.gameObject.Equals(other.gameObject)) {
                    isPlatformed = true;
                    return;
                }
            }
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.25f), -Vector3.up, out hit)) {
                if (hit.collider.gameObject.Equals(other.gameObject)) {
                    isPlatformed = true;
                    return;
                }
            }
            if (Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z - 0.25f), -Vector3.up, out hit)) {
                if (hit.collider.gameObject.Equals(other.gameObject)) {
                    isPlatformed = true;
                    return;
                }
            }
        }
        else if (other.gameObject.tag == "Reset Floor") {
            transform.position = checkpoint;
        }*/
        if (other.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (!(isGrounded || isPlatformed) && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")) {
            OnCollisionEnter(collision);
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Ground") {
            isGrounded = false;
        }
		if (other.gameObject.tag == "Platform") {
			isPlatformed = false;
		}
    }

    void FixedUpdate() {

		if (isGrounded || isPlatformed) {
			int hor = 0;
			int ver = 0;

			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
				ver++;
			}
			if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
				hor--;
			}
			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				hor++;
			}
			if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				ver--;
			}

			if (hor != 0 || ver != 0) {
				float verticalSpeed = rb.velocity.y;
				Vector3 horComponent = Vector3.Normalize (new Vector3 (hor * mainCam.transform.right.x, 0.0f, hor * mainCam.transform.right.z));
				Vector3 verComponent = Vector3.Normalize (new Vector3 (ver * mainCam.transform.up.x, 0.0f, ver * mainCam.transform.up.z));
                Quaternion target = Quaternion.Euler(0, 0, 0);
                if (ver != 0 && hor != 0)
                    target = Quaternion.Euler(0, (hor * 90) - (ver * (hor * 45)), 0);
                else if (ver == -1 && hor == 0)
                    target = Quaternion.Euler(0, -180, 0);
                else 
                    target = Quaternion.Euler(0, (hor * 90), 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 4f);

                rb.velocity = Vector3.Normalize (horComponent + verComponent) * speed;
				rb.velocity = new Vector3 (rb.velocity.x, verticalSpeed, rb.velocity.z);
			} else {
				rb.velocity = new Vector3 (0.0f, rb.velocity.y, 0.0f);
				rb.angularVelocity = Vector3.zero;
			}
			if (Input.GetKey (KeyCode.Space)) {
				rb.velocity += Vector3.up * speed;
			}
		}
    }
}
