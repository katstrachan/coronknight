using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class baseEnemyController : MonoBehaviour
{
    npcAudio audioScript;
    // Start is called before the first frame update
    public NavMeshAgent enemy;
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask beaconLayer;
    private Rigidbody rb;
    public BoxCollider attackBox;
    private npcInternalAction myStats;
    private Animator artist;
    private GameObject closestBeacon;
    private beaconStats myBeacon;
    private float myBeaconRadius;
    private SphereCollider myBeaconRadiusSphere;
    bool posFound;
 
    float waitTimer = 5f;

    float followRadius = 20f;
    float attackRadius = 2f;
  

    //Wander Vars
    private Vector3 wanderToPos = Vector3.zero;
    private bool validPos;
    public Vector3 newPos;
  

    public Vector3 knockBackForce = new Vector3(0f, 0f, 1f);

    //Attack Vars
    int myHitDamage;
    public float attackWaitTime = 1f;
    float attackCooldown;
    
    //Debug
    public bool wanderingAllowed = false;
    public bool followingAllowed = false;
    public bool attackingAllowed = false;
    public ParticleSystem spawnParticles;


    void Start()
    {
        audioScript = GetComponent<npcAudio>();
        Instantiate(spawnParticles, transform.position, Quaternion.identity);
        enemy = GetComponent<NavMeshAgent>();
        //Beacon Set Up

        setWaitTimer();
        //Get Components
        myStats = GetComponent<npcInternalAction>();
        attackBox = GetComponent<BoxCollider>();
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        artist = GetComponent<Animator>();

        //Masks
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLayer = LayerMask.GetMask("player");
        beaconLayer = LayerMask.GetMask("pillar");
        myHitDamage = myStats.getDamageDealt();

        //Set-up
       
        attackBox.enabled = false;
        validPos = false;
        resetAttackCooldown();
        newPos = transform.position;
        myHitDamage = myStats.getDamageDealt();
    }

    // Update is called once per frame
    void Update()
    {
        waitTimerTick();

        //Attack Cooldown
        addAttackCooldownTime();
        animateMovement();

        //Check Spheres
        bool playerSpotted = Physics.CheckSphere(transform.position, followRadius, playerLayer);
        bool playerInReach = Physics.CheckSphere(transform.position, attackRadius, playerLayer);
        bool playerWithinBeacon = Physics.CheckSphere(closestBeacon.transform.position, myBeaconRadius, playerLayer);

        //Check if player has captured our base
        if (player != null)
        {
            //Attack enemy within beacon range and our sight
            if (playerSpotted && playerInReach && attackingAllowed && playerWithinBeacon)
            {
                //if we don't already have a target
                transform.LookAt(player);

                resetAgent();
                if (attackCooldown > attackWaitTime)
                {
                    intendAttack();
                }
            }
            //Follow enemy in sight and beacon range
            else if (playerSpotted && !playerInReach && followingAllowed && playerWithinBeacon)
            {
                artist.SetTrigger("alert");
                transform.LookAt(player);
                followPlayer();

            }
            //Wander
            else if (wanderingAllowed)
            {
                wander();
            }


        }
        else
        {
            resetAgent();
            wander();
        }
    }


    //Increase time between attacks
    void addAttackCooldownTime()
    {
        if (attackCooldown <= attackWaitTime)
        {
            attackCooldown += Time.deltaTime;
        }
    }

    //Animate enemy movement if they're moving
    void animateMovement()
    {
        if (enemy.velocity.x != 0f && enemy.velocity.z != 0f)
        {
            artist.SetBool("moving", true);
        }
        else
        {
            artist.SetBool("moving", false);
        }
    }

    //Make Enemy wander
    

    void wander()
    {

        resetAttackCooldown();
        

        if (!posFound)
        {
            wanderToPos = findNewPos();
        }
        else
        {

            if ((distance(transform.position, wanderToPos)) < 1f)
            {
                posFound = false;
                startWaitTimer();
            }
            else
            {
                
                enemy.SetDestination(wanderToPos);
               
            }
        }

    }

    float distance(Vector3 a, Vector3 b) //O
    {
        return (a - b).magnitude;
    }

    void waitTimerTick()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
        }
        else
        {
            wanderingAllowed = true;
        }
    }

    void startWaitTimer()
    {
        setWaitTimer();
        wanderingAllowed = false;
    }

    void setWaitTimer()
    {
        waitTimer = Random.Range(1, 4);
    }


    float findDistance(Transform otherObject)
    {
        return (transform.position - otherObject.position).sqrMagnitude;
    }


    Vector3 findNewPos() 
    {
      

        NavMeshHit navHit;
        RaycastHit hit;

        bool validPos = false;
        Vector3 sp = new Vector3();

        while (validPos == false)
        {
            Vector3 point = closestBeacon.transform.position + Random.insideUnitSphere * (2 * myBeaconRadius);
            bool pointFound = NavMesh.SamplePosition(point, out navHit, 1f, NavMesh.AllAreas);

            if ((Physics.Raycast(point, -transform.up, out hit, 1.5f)) && (hit.collider.gameObject.CompareTag("ground")) && (pointFound == true))
            {
                posFound = true;
                sp = navHit.position;
                validPos = true;
            }

        }
        return sp;


    }
 

    void followPlayer()
    {
        resetAttackCooldown();
        if (enemy != null)
        {
            transform.LookAt(player);
            Vector3 playerPos = new Vector3(player.position.x, player.position.y, player.position.z);
            enemy.SetDestination(playerPos);
        }
        else
        {
            resetAgent();
            wander();
        }


    }

    void intendAttack()
    {

        attackCooldown = 0f;
        artist.SetTrigger("attacking");


    }
    void resetAttackCooldown()
    {
        attackCooldown = attackWaitTime + 1;
    }

    void attackAlert()
    {
        attackBox.enabled = !attackBox.enabled;
    }

    public void OnTriggerEnter(Collider col)
    {
        //shield hit
        if (col.gameObject.CompareTag("Player") && (col.GetType() == typeof(BoxCollider)))
        {
            
            applyKnockBack();
        }
        else if (col.gameObject.CompareTag("Player") && (col.GetType() == typeof(SphereCollider)))
        {
            //attack player
            audioScript.makeAttackNoise();
            playerInternalAction playerStats = col.gameObject.GetComponent<playerInternalAction>();
            playerStats.applyDamage(myHitDamage);

        }
    }

    void resetAgent()
    {
        enemy.enabled = false;
        enemy.enabled = true;
    }


    public void applyKnockBack()
    {

        artist.SetTrigger("knockBack");

        rb.MovePosition(transform.position + knockBackForce);
    }

    public void killCharacter()
    {
        myStats.destroyCharacter(closestBeacon);
    }

    public void setSpawnPoint(GameObject beacon)
    {
        closestBeacon = beacon;
        myBeacon = closestBeacon.GetComponent<beaconStats>(); //for reclaiming the beacon  
        myBeaconRadiusSphere = closestBeacon.transform.Find("beaconAOI").GetComponent<SphereCollider>(); //for radius checking
        myBeaconRadius = myBeaconRadiusSphere.radius;
    }
}
