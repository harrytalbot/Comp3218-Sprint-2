using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionCone : MonoBehaviour {

    public float coneAngle;
    public float coneDistance;
    public float segments = 2;
    private float segmentAngle;
    private GameObject[] targets;
    private Mesh myMesh;

    private Vector3[] verts;
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uvs;

    private float actualAngle;
    

	// Use this for initialization
	void Start () {
        targets = GameState.GetCharacters(); // Assuming characters are not added or removed during the game.

        myMesh = gameObject.GetComponent<MeshFilter>().mesh;
        myMesh.Clear();
        actualAngle = 90.0f - coneAngle;
        segmentAngle = coneAngle * 2 / segments;
        verts = new Vector3[(int)segments * 3];
        normals = new Vector3[(int)segments * 3];
        triangles = new int[(int)segments * 3];
        uvs = new Vector2[(int)segments * 3];

        for (int i = 0; i < verts.Length; i++) {
            verts[i] = new Vector3(0, 0, 0);
            normals[i] = Vector3.up;
        }
        float a = actualAngle;
        for (int i = 1; i < verts.Length; i += 3) {
            verts[i] = new Vector3( Mathf.Cos(Mathf.Deg2Rad * a) * coneDistance,
                                    0,
                                    Mathf.Sin(Mathf.Deg2Rad * a) * coneDistance);
            a += segmentAngle;
            verts[i+1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * coneDistance,
                                    0,
                                    Mathf.Sin(Mathf.Deg2Rad * a) * coneDistance);
        }

        for (int i = 0; i < triangles.Length; i += 3) {
            triangles[i] = 0;
            triangles[i + 1] = i + 2;
            triangles[i + 2] = i + 1;
        }

        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(verts[i].x, verts[i].z);
        }

        myMesh.vertices = verts;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uvs;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDirection;
        float targetDistance;
        float angle;

        for (int i = 0; i < targets.Length; i++) {
            if (targets[i] != null) {
                targetDirection = targets[i].transform.position - transform.position;
                targetDistance = targetDirection.magnitude;
                angle = Vector3.Angle(targetDirection, transform.forward);

                if (angle <= coneAngle && targetDistance <= coneDistance)
                    Debug.Log("I've spotted " + targets[i].name);
            }
        }
        
    }
}
