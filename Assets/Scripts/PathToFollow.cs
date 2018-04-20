using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.AI;

public class PathToFollow : MonoBehaviour {

    public Transform[] wayPointList;
    public float minDist;
    public Transform path;


    private int destPoint = -1;
    public NavMeshAgent agent;

    public float speed = 2f;

    // Use this for initialization
    void Start()
    {

        path = transform.parent.Find("Paths");

        wayPointList = path.Cast<Transform>().ToArray();

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();

    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (wayPointList.Length == 0)
            return;

        if (destPoint < wayPointList.Length - 1)
        {
            // Set the agent to go to the currently selected destination.
            destPoint++;
            agent.destination = wayPointList[destPoint].position;
            print(destPoint);
        } else
        {
            destPoint = 0;
            agent.destination = wayPointList[destPoint].position;
        }
        //Just choosing the next point (different to tutorial)
       // destPoint = Mathf.Max(wayPointList.Length - 1, ++destPoint);
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (agent.remainingDistance < minDist)
            GotoNextPoint();
    }


    /*
    // Update is called once per frame
    void Update()
    {
        // check if we have somewere to walk
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
                targetWayPoint = wayPointList[currentWayPoint];
           // walk();
        }
        else if (currentWayPoint > this.wayPointList.Length)
        {
            if (targetWayPoint == null)
                targetWayPoint = wayPointList[0];
            //walk();
        }


        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, step);

        float i = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetWayPoint.position.x, targetWayPoint.position.z));

        print(i);

        if (i < 1)
        {
            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }

    }

    void walk()
    {
        // rotate towards the target
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            targetWayPoint = wayPointList[currentWayPoint];
        }
        
    }
    */
}
