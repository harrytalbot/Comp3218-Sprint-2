using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshGenerator : MonoBehaviour {

    public Material material;

    public float height, width, depth;

	// Use this for initialization
	void Start () {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[8];

        // FRONT TRIANGLE
        // top
        vertices[0] = new Vector3(0, height, depth / 2);
        // right
        vertices[1] = new Vector3(width/2, 0, depth / 2);
        // bottom
        vertices[2] = new Vector3(0, 0, depth / 2);
        // left
        vertices[3] = new Vector3(-(width/2), 0, depth / 2);

        // REAR TRIANGLE
        // top
        vertices[4] = new Vector3(0, height, -(depth / 2));
        // right
        vertices[5] = new Vector3(width / 2, 0, -(depth / 2));
        // bottom
        vertices[6] = new Vector3(0, 0, -(depth / 2));
        // left
        vertices[7] = new Vector3(-(width / 2), 0, -(depth / 2));


        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7};

        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshFilter>().mesh = mesh;



    }

    // Update is called once per frame
    void Update () {
		
	}
}
