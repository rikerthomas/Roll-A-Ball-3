using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemyPos;
    //repeateRate is how many seconds before the next enemy spawns.
    private float repeateRate = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //if the player collides with the game object, then spawn enemies
        if(other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 0.5f, repeateRate);
            Destroy(gameObject, 21);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    void EnemySpawner()
    {
        //This is where they will spawn, and instantiate clones the game object of my choice.
        Instantiate(enemyPos, enemyPos.position, enemyPos.rotation);
    }


}
