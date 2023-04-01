using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Fellow : MonoBehaviour
{
    [SerializeField]
    float speed = 0.05f;

    [SerializeField]
    Ghost ghost;

    public int score = 0;
    public int pelletsEaten = 0;
    public int life = 3;

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    GameObject[] ghosts;



    [SerializeField]
    int pointsPerPellet = 100;
    Vector3 orginalPosition;

    [SerializeField]
    public float powerupDuration = 10.0f; // How long should powerups last ?

    public float powerupTime = 0.0f; // How long is left on the current powerup ?

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
            pelletsEaten++;
            score += pointsPerPellet;
            Debug.Log(" Score is " + score);
            Debug.Log("Pellets eaten: " + pelletsEaten);            

        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            powerupTime = powerupDuration;
            Debug.Log(" Score is " + score);

        }

        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
            transform.position = new Vector3(14.5f, 1, 7);
        }
        else if (other.gameObject.CompareTag("RightTeleporter"))
        {
            transform.position = new Vector3(1, 1, 7);
        }

    }

    public bool PowerupActive()
    {
        return powerupTime > 0.0f;
    }
    public bool PowerupActive(bool powerup)
    {
        powerupTime = 0;
        return powerup;
    }

    public int PelletsEaten()
    {
        return pelletsEaten;
    }

    // Start is called before the first frame update
    void Start()
    {
        ghosts= GameObject.FindGameObjectsWithTag("Ghost");
        orginalPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ghost"))
        {
            if (!PowerupActive())
            {
                Debug.Log("You died !");

                foreach (GameObject phantom in ghosts)
                {
                    UnityEngine.AI.NavMeshAgent agent;
                    agent = phantom.GetComponent<NavMeshAgent>();
                    ghost.GhostReset(agent);
                }

                life = life - 1;
                if (life == 2)
                {
                    life3 = GameObject.Find("Life3");
                    life3.SetActive(false);
                }
                else if (life == 1) {
                    life2 = GameObject.Find("Life2");
                    life2.SetActive(false);
                }
                else if (life == 0)
                {
                    life1 = GameObject.Find("Life1");
                    life1.SetActive(false);
                    AudioClip gameOverAudio;
                    AudioSource audioSrc;
                    gameOverAudio = Resources.Load<AudioClip>("gameover");
                    audioSrc = GetComponent<AudioSource>();
                    audioSrc.volume = 1f;
                    audioSrc.PlayOneShot(gameOverAudio);

                }

                transform.position = orginalPosition;
            }
                
        }

    }

   

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Rigidbody b = GetComponent<Rigidbody>();
        Vector3 velocity = b.velocity;
        
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed;
        }
        b.velocity = velocity;
    }

    void Update()
    {
        powerupTime = Mathf.Max(0.0f, powerupTime - Time.deltaTime);
    }

    public void FellowLevelUp() {
        if (powerupDuration != 3) {
            powerupDuration = powerupDuration - 1;
            transform.position = orginalPosition;
        }
        
        PowerupActive(false);

    }

}

    


