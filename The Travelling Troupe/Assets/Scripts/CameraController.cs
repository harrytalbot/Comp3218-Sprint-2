using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform targetToFollow;
    public Vector3 targetOffset;

    private void LateUpdate() {
        transform.position = targetToFollow.position + targetOffset;
    }
}
