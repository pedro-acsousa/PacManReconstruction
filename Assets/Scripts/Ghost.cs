using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    [SerializeField]
    Material scaredMaterial;
    [SerializeField]
    Material normalMaterial;
    [SerializeField]
    Fellow player;



    bool evaluateCollision;

    bool pinkydead;

    public GameObject[] ghosts;

    YellowFellowGame game;
    float levelspeed = 3.5f;
    float secondsCount = 1.5f;
    float inkyCount = 0f;




    public UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = PickRandomPosition();
        normalMaterial = GetComponent<Renderer>().material;

        ghosts = GameObject.FindGameObjectsWithTag("Ghost");

       
    }

    //Create a timer for ghost release
    public float timeRemaining = 10;

    private void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("Player")) {
            evaluateCollision = true;
        }
    }

    Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.z += randomDirection.y;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(destination,out navHit ,8.0f, UnityEngine.AI.NavMesh.AllAreas );
        return navHit.position ;
    }

    


    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info ))
        {
            if(info.transform.CompareTag ("Player"))
            {
                return true; // Ghost sees the player
            }
        }
        return false;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, UnityEngine.AI.NavMesh.AllAreas);
        return navHit.position;
    }

    public bool SendToCage() {
        
        Vector3 destination = new Vector3(7.612969f, 0.8f, 6.514441f);
        agent.destination = destination;
        Debug.Log("Player has killed " + this.name);
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    


        return true;
    }



    bool hiding = false; //A new member variable !
    // Update is called once per frame
    void Update()
    {
        inkyCount -= Time.deltaTime;
        if (inkyCount<=0f && this.name == "Inky" || this.name=="InkyClyde" || this.name == "InkyBlinky" || this.name =="InkyPinky") {
            if (inkyCount <= 0f)
            {
                int random = Random.Range(1, 4);

                if (random == 1) {
                    inkyCount = 10f;
                    this.name = "InkyClyde";
                    Debug.Log("I am Clyde!");
                } else if (random == 2)
                {
                    inkyCount = 10f;
                    this.name = "InkyBlinky";
                    Debug.Log("I am Blinky!");
                }
                else if (random == 3)
                {
                    inkyCount = 10f;
                    this.name = "InkyPinky";
                    Debug.Log("I am Pinky!");
                }

            }


        }

        if (this.name == "Clyde" || this.name=="InkyClyde")
        {
            agent.speed = levelspeed - 0.2f; // Clyde speed is less than other ghosts'
        }

        if (this.name == "Pinky" || this.name == "InkyPinky")
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= 2f)
            {
                agent.speed = 0f; //Pinky becomes dead for 1.5 seconds after being in a 2f radius of the player
                pinkydead = true;
            }
            secondsCount -= Time.deltaTime;
            if (secondsCount <= 0f)
            {
                agent.speed = levelspeed; //After the counter, Pinky's speed returns to normal and the dead timer is reset
                secondsCount = 1.5f;
                pinkydead = false;
            }
        }

        if (CanSeePlayer() && !player.PowerupActive() && this.name != "Clyde" && !pinkydead)
        {
            Debug.Log("I can see you!");
            agent.destination = player.transform.position;

            if (this.name == "Blinky" || this.name == "InkyBlinky")
            {
                agent.speed = levelspeed + 1.7f; // Blinky becomes enraged when it sees the player massively increasing speed
            }

            if (this.name == "Pinky" || this.name == "InkyPinky")
            {

                agent.speed = levelspeed + 0.8f; // Pinky becomes enraged when it sees the player and increases speed

            }
        }
        else
        {
            if (agent.remainingDistance < 0.5f || (this.name == "Blinky" && agent.remainingDistance < 1f) || (this.name == "InkyBlinky" && agent.remainingDistance < 1f))
            {
                agent.destination = PickRandomPosition();


                if (this.name == "Blinky" || this.name == "InkyBlinky")
                {
                    agent.speed = levelspeed; // Blinky is no longer enraged
                }
                if (this.name == "Pinky" || this.name == "InkyPinky")
                {
                    agent.speed = levelspeed - 0.8f; // Pinky is no longer enraged
                }
            }
        }


        if (player.PowerupActive())
        {
            Debug.Log(" Hiding from Player !");
            if (!hiding || agent.remainingDistance < 0.5f)
            {
                hiding = true;
                agent.destination = PickHidingPlace();
                GetComponent<Renderer>().material = scaredMaterial;
            }

            if (evaluateCollision) { 
                SendToCage();
                player.score = player.score + 500;
                evaluateCollision = false;
                
            }
        }
        else
        {
            Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), false);
            //Debug.Log(" Chasing Player !");
            if (hiding)
            {
                GetComponent<Renderer>().material = normalMaterial;
                hiding = false;
            }
        }

        if (player.PowerupActive())
        {
            GetComponent<Renderer>().material = scaredMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = normalMaterial;
        }

    }

    void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.CompareTag("LeftTeleporter"))
        {

            agent.Warp(new Vector3(14.5f, 0.8f, 7));

        }
        else if (coll.gameObject.CompareTag("RightTeleporter"))
        {
            agent.Warp(new Vector3(1, 0.8f, 7));
        }

    }

    public void GhostReset(UnityEngine.AI.NavMeshAgent agent)
    {
        agent.Warp(new Vector3(7.612969f, 0.8f, 6.514441f));
    }
    public void GhostSpeedLevelUp(UnityEngine.AI.NavMeshAgent agent)
    {
        
        agent.speed += 0.5f;
        levelspeed = levelspeed + 0.5f;
    }
}
