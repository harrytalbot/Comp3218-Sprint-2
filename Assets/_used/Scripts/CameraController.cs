using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Animator anim;
    private PlayerController pc;

    public void Awake() {
        pc = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
        //anim = GetComponentInChildren<Animator>();
    }

    // doesn't work right now
    /*public void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Find("MainCamera").position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Find("MainCamera").position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        }
    }

    
     private void LateUpdate() { }
        if (!pc.isTalking)
            transform.position = targetToFollow.position + targetOffset;
    }
    */


    private void LateUpdate()
    {
        if (transform.parent.gameObject.name.Contains("Donkey")) {
            if (!GameState.isTalking && GameState.activeCharacter == pc.characterNumber && anim.GetCurrentAnimatorStateInfo(0).IsName("DonkeyTalkAnim"))
                anim.Play("ExitTalkAnim");
            else if (GameState.isTalking && GameState.activeCharacter == pc.characterNumber)
                anim.Play("DonkeyTalkAnim");
        }
        else if (transform.parent.gameObject.name.Contains("Cat")) {
            if (!GameState.isTalking && GameState.activeCharacter == pc.characterNumber && anim.GetCurrentAnimatorStateInfo(0).IsName("CatTalkAnim"))
                anim.Play("ExitTalkAnim");
            else if (GameState.isTalking && GameState.activeCharacter == pc.characterNumber)
                anim.Play("CatTalkAnim");
        }
        
        //else if (!GameState.isTalking)
        //transform.position = targetToFollow.position + targetOffset;
    }

}
