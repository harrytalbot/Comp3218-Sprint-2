using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform targetToFollow;
    public Vector3 targetOffset;
    public Animator anim;

    private PlayerController pc;

    public void Awake() {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void LateUpdate() {

        if (!pc.isTalking && anim.GetCurrentAnimatorStateInfo(0).IsName("TalkAnim"))
            anim.Play("ExitTalkAnim");
        else if (!pc.isTalking)
            transform.position = targetToFollow.position + targetOffset;
        else 
            anim.Play("TalkAnim");
    }
}
