using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    //These are the variables for the different text that is shown on screen
    public TextMeshProUGUI countText;
    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI restartInfo;
    public TextMeshProUGUI helpInfo;
    //Variables for when info about the game will an will not show
    public float restartInfoEnd;
    public float restartInfoStart;
    public GameObject winTextObject;
    public GameObject wall;     //Setting the wall as a variable to be able to turn it on and off when needed
    public int enemyHits;    //Variable to keep how many times the player has been hit
    public AudioClip audioClip;     //this allows me to get the audio clip that I need to play when the player reaches the end point of the game
    public ParticleSystem hitParticles;   //PArticles that play when the player is hit
    public ParticleSystem winParticles;  //this is how the particles play when the player reaches the end point of the game.
    public AudioClip collisionAudio; //Variable of what the audio is when the player is hit.
    AudioSource audioSource;  //This audio source is what allows the audio to play.
    AudioSource collisionSource;
    public float reloadDelay = 5f; //Setting how long the delay is before the game restarts if the palyer is defeated.
    
    private float movementX;
    private float movementY;
    public PlayerInput input;

    private Rigidbody rb;
    public int count;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        //This sets the enemyHits text to zero, which is how many times the player has been hit at the start of the game.
        enemyHits = 0;
        SetCountText();
        winTextObject.SetActive(false);
        helpInfo.enabled = false;
        //this gets the components of the audio source and particle systems which will allow them both to play at the correct time.
        audioSource = GetComponent<AudioSource>();
        restartInfoEnd = 10f;
        collisionSource = GetComponent<AudioSource>();
        input = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        //If the count is 33 then a new path unlocks allowing the player to finish the game. 
        if (count >= 33)
        {
            Destroy(wall);
        }
        //This makes it so that the restart info in the game is shown to the player for 10 seconds,
        //and then after that, it dissapears, as to not clutter the screen. 
        if (restartInfo.isActiveAndEnabled && (Time.time >= restartInfoEnd))
        {
            restartInfo.enabled = false;
        }
        //Movement method created to make the code look a bit cleaner.
        Movement();
    }

    public void Movement()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    IEnumerator RestartLevelAfterWait()
    {//Method that is needed for the Coroutine to be able to work. Waits for 3 seconds, then gets the build index and loads the current scene again.
        yield return new WaitForSeconds(3.0f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

    public void OnCollisionEnter(Collision collision)
    {
        //This says that if the player collides with either enemy, and the number of hits reaches ten, the game will restart.
        if (collision.gameObject.tag == "Enemy1" || collision.gameObject.tag == "Enemy2")
        {
            //this increases the count of hits that the player has taken.
            enemyHits = enemyHits + 1;
            SetCountText();
            hitParticles.Play(); //particles play if the player is hit by an enemy
            collisionSource.Stop(); //This prevents the audio from playing more than once on a 1 to 1 hit ratio.
            collisionSource.PlayOneShot(collisionAudio); //plays the collision audio clip
            if (enemyHits >= 10)
            {
                input.enabled = false; //disables movement so the player knows that something is wrong.
                StartCoroutine(RestartLevelAfterWait()); //Starts the Coroutine to restart the level, while also adding a few seconds of delay.
            }
            if (enemyHits >= 11)
            {
                hitsText.text = "Enemy Hits: " + 10.ToString(); //Keeps the player hits at 10 while the Coroutine waits for the three seconds to restart the level.
                collisionSource.enabled = false; //Disables collisions so that the audio and particles do not continue to play.
                hitParticles.Stop(); //Added to stop the particles on the last hit.
            }
        }

        //This works because it checks to see if the collision has happened, and if the count is correct, then everything listed will happen.
        if (collision.gameObject.CompareTag("Finish"))
        {
            //If the count is equal to 31, then the you win text will show up, and the audio and particles will play. 
            if (count >= 34)
            {
                winParticles.Play();
                winTextObject.SetActive(true);//Sets the win text to true
                audioSource.Stop();//prevents audio from playing mroe than once
                audioSource.PlayOneShot(audioClip);//plays the audio clip
            }
            else if(count < 33)
            {
                helpInfo.enabled = true; //If the player hits the pad before getting 33 collectibles then some info will show telling them how to unlock the path.
                //there are two objects where the tag is finish, but only one is actually the real finish, which is at the end of the game.
                if(helpInfo.isActiveAndEnabled && (Time.time >= restartInfoEnd))
                {//After help info is activated, it will turn off after 10 seconds.
                    helpInfo.enabled = false;
                }
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        //this increases the enemyHits text, so that the player has an accurate count of how many times they have been hit.
        hitsText.text = "Enemy Hits: " + enemyHits.ToString();
        countText.text = "Count: " + count.ToString();
    }
}
