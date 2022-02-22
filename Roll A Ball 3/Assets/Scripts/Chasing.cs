using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Chasing : MonoBehaviour
{
    public NavMeshAgent enemy;
    public GameObject Player;
    public float enemyDistance = 4.0f;
    public Material material;
    public Transform returnToPatrol;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        returnToPatrol = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        //If the enemy is less than 4 spaces away from the player, the enemy will change color to blue, to indicate chasing, and will chase the player. 
        if(distance < enemyDistance)
        {
            //This brings the enemy towards the player
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position - dirToPlayer;
            enemy.SetDestination(newPos);
            GetComponent<MeshRenderer>().material.color = Color.blue; //sets the enemy color to blue to indicate that their state has changed. 
        }
        //If the player is more than 4 units away, the enemy will return to it's original color. 
        else if (distance > enemyDistance)
        {
            GetComponent<MeshRenderer>().material = material; //If the player is far away enough the enemy will go to their next point and their color will reset.
            Patrolling.FindObjectOfType<Patrolling>().GotoNextPoint();
        }
    }
}
