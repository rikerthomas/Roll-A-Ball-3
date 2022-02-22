using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    //This is a list of points, which will be empy game objects, where the enemy will patrol to
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //autoBreaking makes it so that the agent slows down when arriving to the point, making things look not so jittery.
        agent.autoBraking = true;
        GotoNextPoint();
    }
    public void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        //This is what tells the agent to go to the first destination.
        agent.destination = points[destPoint].position;
        //destPoint is the destination point plus one and the remainder of the points length. 
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //This is to make it so that the next point is queued more quickly in the code, so that it happens faster. 
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
}
