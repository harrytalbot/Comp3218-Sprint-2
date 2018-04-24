using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeToHouse : MonoBehaviour {

    private bool inHouse = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inHouse = true;
            MeshRenderer[] rs = transform.Find("Roof").GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in rs)
                r.enabled = false;
        }
     }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inHouse = false;
            MeshRenderer[] rs = transform.Find("Roof").GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in rs)
                r.enabled = true;
        }
    }
}
