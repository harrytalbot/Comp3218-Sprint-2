using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyVisionCone : MonoBehaviour {

    public float coneAngle;
    public float coneDistance;
    public bool sweep;
    public float sweepAngle;
    public float sweepSpeed;
    public float segments = 2;
    public float maxChaseDistance;
    public float minChaseDistance;
    private float segmentAngle;
    private GameObject[] targets;
    private Mesh myMesh;
    public Material material;
    private Color originalColor;

    private Vector3[] verts;
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uvs;

    private float actualAngle;
    private Vector3 originalDirection;
    private Vector3 originalSpot;

    private RaycastHit shot;
    private bool detected = false;
    private GameObject chaseTarget;
    private GameObject enemyObject;

    public bool alerted = false;
    private bool distracted = false;
    private float distractTime = 0;
    public GameObject knocked;
    

	// Use this for initialization
	void Start () {
        enemyObject = transform.parent.gameObject;
        targets = GameState.GetCharacters(); // Assuming characters are not added or removed during the game.
        material = GetComponent<Renderer>().material;
        originalDirection = transform.forward;
        originalSpot = transform.position;
        originalColor = material.color;
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


        if (detected) {
            float chaseDistance = enemyObject.GetComponent<NavMeshAgent>().remainingDistance;
            float straightLineDistance = Vector3.Distance(enemyObject.transform.position, chaseTarget.transform.position);
            if (chaseDistance > minChaseDistance && chaseDistance < maxChaseDistance) { // He's still chasing you!
                enemyObject.GetComponent<NavMeshAgent>().SetDestination(chaseTarget.transform.position);
            }
            else if (chaseDistance < minChaseDistance && straightLineDistance > minChaseDistance) {
                enemyObject.GetComponent<NavMeshAgent>().SetDestination(chaseTarget.transform.position);
            }
            else if (chaseDistance >= maxChaseDistance && chaseDistance != Mathf.Infinity) { // You escaped!
                detected = false;
                material.color = originalColor;
                enemyObject.GetComponent<NavMeshAgent>().SetDestination(originalSpot);
            }
            else if (chaseDistance <= minChaseDistance && straightLineDistance <= minChaseDistance) { // You were caught!
                enemyObject.GetComponent<NavMeshAgent>().SetDestination(enemyObject.transform.position);
                enemyObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                material.color = Color.black;
                if (SceneManager.GetActiveScene().name.Contains("Shed"))
                    Initiate.Fade("ShedCaughtEnding", Color.black, 1);
                else if (SceneManager.GetActiveScene().name.Contains("House"))
                    Initiate.Fade("HouseCaughtEnding", Color.black, 1);
            }
        }
        else if (distracted && !alerted) {

        }
        else if (alerted && enemyObject.GetComponent<NavMeshAgent>().desiredVelocity.Equals(new Vector3(0, 0, 0))) {
            transform.parent.LookAt(knocked.transform);
            transform.parent.eulerAngles = new Vector3(0, transform.parent.eulerAngles.y, 0);
        }
        else if (!detected && enemyObject.GetComponent<NavMeshAgent>().desiredVelocity.Equals(new Vector3(0, 0, 0)) && !transform.forward.Equals(originalDirection)) { // Reset orientation
            transform.parent.forward = originalDirection;
            enemyObject.GetComponent<NavMeshAgent>().SetDestination(transform.parent.position);
        }

        Vector3 frontDirection = transform.parent.forward;

        if (!detected) {
            enemyObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            for (int i = 0; i < targets.Length; i++) {
                if (targets[i] != null) {
                    targetDirection = targets[i].transform.position - transform.position;
                    targetDistance = targetDirection.magnitude;
                    angle = Vector3.Angle(targetDirection, transform.forward);
                    Vector3 beforeDirection = transform.forward;
                    if (angle <= coneAngle && targetDistance <= coneDistance) {
                        transform.LookAt(targets[i].transform);
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot)) {
                            if (shot.collider.gameObject.Equals(targets[i]) || shot.collider.transform.parent.gameObject.Equals(targets[i])) {
                                material.color = Color.red;
                                chaseTarget = targets[i];
                                Debug.Log(chaseTarget.name);
                                detected = true;
                                enemyObject.GetComponent<NavMeshAgent>().SetDestination(chaseTarget.transform.position);
                                transform.forward = frontDirection;
                                if (alerted || distracted) {
                                    alerted = false;
                                    distracted = false;
                                }
                            }
                            else
                                transform.forward = beforeDirection;
                        }
                    }
                }
            }
            if (!distracted && sweep && Vector3.Angle(transform.forward, frontDirection) < sweepAngle)
                transform.Rotate(Vector3.up, sweepSpeed * Time.deltaTime);
            else if (!distracted && sweep) {
                sweepSpeed = -sweepSpeed;
                while (Vector3.Angle(transform.forward, frontDirection) > sweepAngle) {
                    transform.Rotate(Vector3.up, sweepSpeed * 0.01f);
                }

            }
        }
        if (distracted && distractTime > 0) {
            distractTime -= Time.deltaTime;
        }
        else if (distracted && distractTime <= 0) {
            distracted = false;
        }
    }

    public void GetScared() {
        enemyObject.GetComponent<NavMeshAgent>().SetDestination(new Vector3(0, 0, 75));
        enemyObject.GetComponent<NavMeshAgent>().speed *= 5;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        enabled = false;
    }

    public void GetDistracted(Transform duck, float time) {
        if (!distracted) {
            distracted = true;
            transform.parent.LookAt(duck.transform);
            transform.parent.eulerAngles = new Vector3(0, transform.parent.eulerAngles.y, 0);
            distractTime = time;
        }        
    }
}
