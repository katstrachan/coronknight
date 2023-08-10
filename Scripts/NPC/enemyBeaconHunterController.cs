using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class enemyBeaconHunterController : MonoBehaviour
{
    npcAudio audioScript;
    public NavMeshAgent enemy;
    public Transform player;
    private bool playerDed = false;
    public LayerMask playerLayer;
    public LayerMask beaconLayer;
    public LayerMask allyLayer;
    private Rigidbody rb;
    public BoxCollider attackBox;
    private npcInternalAction myStats;
    private Animator artist;
    private GameObject targetBeacon;
    beaconStats targetBeaconStatus;
    float waitTimer = 5f;
    bool foundAllyTarget = false;
    Transform allyTarget;
    bool coinTossLost = false;
    bool isBeingAttacked = false;

    private float followRadius = 20f;
    private float attackRadius = 1.5f;

    //Wander Vars
    private Vector3 wanderToPos = Vector3.zero;
    private bool validPos;
    public Vector3 newPos;
    int xVar;
    int zVar;

    public Vector3 knockBackForce = new Vector3(0f, 0f, 1f);

    //Attack Vars
    int myHitDamage;
    public float attackWaitTime = 1f;
    float attackCooldown;
  

    //hunting vars
    bool capturedIntendedBeacon;
    bool beginHunting;
    float attackChance;


    //Debug
    private bool wanderingAllowed = true;
    private bool followingAllowed = true;
    private bool attackingAllowed = true;

    float victoryTimer = 1.66f;
    bool victoryWiggle = false;
    bool playerDedDance = false;
    public ParticleSystem spawnParticles;

    GameObject closestSpawnBox;
    



    void Start()
    {
        audioScript = GetComponent<npcAudio>();
        LayerMask spawnBoxLayer = LayerMask.GetMask("spawnBoxes");
        Instantiate(spawnParticles, transform.position, Quaternion.identity);
        //Beacon Set Up
        setMyTargetBeacon();
        setWaitTimer();


        //Get Components
        myStats = GetComponent<npcInternalAction>();
        attackBox = GetComponent<BoxCollider>();
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        artist = GetComponent<Animator>();
        targetBeaconStatus = targetBeacon.GetComponent<beaconStats>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLayer = LayerMask.GetMask("player");
        allyLayer = LayerMask.GetMask("ally");
        beaconLayer = LayerMask.GetMask("pillar");
        myHitDamage = myStats.getDamageDealt();

        //Set-up
        xVar = 20;
        zVar = 20;
        attackBox.enabled = false;
        validPos = false;
        resetAttackCooldown();
        newPos = transform.position;
        capturedIntendedBeacon = false;
        beginHunting = true;
        attackChance = Random.Range(0f, 10f);
        
    }

    // Update is called once per frame
    void Update()
    {
    
        waitTimerTick();
        addAttackCooldownTime();
        animateMovement();
        if(playerDedDance)
        {
            startVictoryTimer();
        }

        if (victoryWiggle)
        {
            if (victoryTimer > 0)
            {
                victoryTimer -= Time.deltaTime;
            }
            else
            {
                victoryWiggle = false;
            }
        }
        else
        {

            if (capturedIntendedBeacon)
            {
                
                idleMode();
            }
         
            else
            {
                
                hunterMode();
            }
        }
       
    }

  

    float findDistance(Transform otherObject)
    {
        return (transform.position - otherObject.position).sqrMagnitude;
    }

    void setMyTargetBeacon()
    {
        List<GameObject> playerBeacons = new List<GameObject>();
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("beacon"); 
        
        float d = 1000000;
        for(int i = 0; i< beacons.Length; i++)
        {
            bool playerControlled = beacons[i].GetComponent<beaconStats>().getIsPlayerCaptured();
            if(playerControlled)
            {
                float checkD = (transform.position - beacons[i].gameObject.transform.position).sqrMagnitude;
                if(checkD <= d)
                {
                    targetBeacon = beacons[i];
                    d = checkD;
                   
                }
            }
          
        }
       
 
    }

    void hunterMode()
    {
        bool playerSpotted;
        bool playerInReach;
        bool allySpotted;
        bool allyInReach;
        bool beaconInReach;
        


        if (beginHunting == true && !isBeingAttacked)
        {
            beginHunting = false;
            transform.LookAt(targetBeacon.transform);
            enemy.SetDestination(targetBeacon.transform.position);
        }

        beaconInReach = checkAttackDist(beaconLayer);
        playerSpotted = checkFollowDist(playerLayer);
        playerInReach = checkAttackDist(playerLayer);
        allySpotted = checkFollowDist(allyLayer);
        allyInReach = checkAttackDist(allyLayer);
        if (!targetBeaconStatus.getIsPlayerCaptured())
        {
            capturedIntendedBeacon = true;
            resetAgent();
        }
        else
        {


            if (beaconInReach)
            {

                transform.LookAt(targetBeacon.transform);
                intendAttack();
            }
            else
            {
                isBeingAttacked = myStats.getBeingAttacked();
                float coinToss = 0f;
                if (!coinTossLost)
                {

                    coinToss = Random.Range(0f, 10f);
                }

                if (playerInReach && isBeingAttacked)
                {
  
                    if (coinToss < attackChance)
                    {
                        coinTossLost = true;

                        capturedIntendedBeacon = true;
                        resetAgent();
                    }
                }
                else if (allyInReach && isBeingAttacked)
                {
                    if (coinToss < attackChance)
                    {
                        coinTossLost = true;
                        beginHunting = true;
                        resetAgent();
                        attackAlly();
                    }

                }
            }
        }
    }

    bool checkFollowDist(LayerMask objLayer)
    {
        return Physics.CheckSphere(transform.position, followRadius, objLayer);
    }

    bool checkAttackDist(LayerMask objLayer)
    {
        return Physics.CheckSphere(transform.position, attackRadius, objLayer);
    }
    void idleMode()
    {

        bool playerSpotted = checkFollowDist(playerLayer);
        bool playerInReach = checkAttackDist(playerLayer);
        bool allySpotted = checkFollowDist(allyLayer);
        bool allyInReach = checkAttackDist(allyLayer);



        if (!playerDed)
        {
            //Attack
            if (playerSpotted && playerInReach && attackingAllowed)
            {
               
                    transform.LookAt(player);

                    resetAgent();
                    if (attackCooldown > attackWaitTime)
                    {

                        intendAttack();
                    }
                
                

            }
           
            //Follow
            else if (playerSpotted && !playerInReach && followingAllowed)
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
        
            wander();
        }
        
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







    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
     
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, (attackRadius + 2.5f));

    }

    void attackAlly()
    {
        if(!foundAllyTarget)
        {
            allyTarget = findAllyTarget();
        }

        if (allyTarget == null)
        {
            allyHasDied();
        }
        else
        {
            transform.LookAt(allyTarget.transform);
            intendAttack();
        }
    }

    Transform findAllyTarget()
    {
        Collider[] allies = Physics.OverlapSphere(transform.position, (attackRadius + 2.5f));
        if (allies.Length != 0)
        {
            return allies[0].transform;
        }
        else
        {
            return null;
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

        if (!validPos)
        {
            wanderToPos = findNewPos();
        }
        else
        {
            enemy.SetDestination(wanderToPos);
        }

        if ((distance(transform.position, wanderToPos)) < 1f)
        {
            startWaitTimer();
            validPos = false;
        }


    }


    float distance(Vector3 a, Vector3 b) 
    {
        return (a - b).magnitude;
    }


    Vector3 findNewPos() 
    {
        float xOffset = (Random.Range(-xVar, xVar));

        float zOffset = (Random.Range(-zVar, zVar));

        
        xOffset += transform.position.x;
        zOffset += transform.position.z;
        newPos = new Vector3(xOffset, transform.position.y, zOffset);
        checkNewPosValid(newPos);
        return newPos;

    }
    void checkNewPosValid(Vector3 pos) 
    {
        NavMeshHit navHit;
        RaycastHit hit;

        bool blockedPath = NavMesh.SamplePosition(pos, out navHit, 1f, NavMesh.AllAreas);

        if ((Physics.Raycast(pos, -transform.up, out hit, 1.5f)) && (hit.collider.gameObject.CompareTag("ground")) && (blockedPath == true))
        {
            newPos = navHit.position;
            validPos = true;
        }
        else
        {
            validPos = false;
        }

    }

    void followPlayer()
    {
        resetAttackCooldown();
        if (!playerDed)
        {
            transform.LookAt(player);
            enemy.SetDestination(player.position);
        }
        else
        {
            resetAgent();
            wander();
        }


    }

    void intendAttack()
    {
      
        resetAgent();
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
        else if (col.gameObject.CompareTag("Player") && (col.GetType() == typeof(SphereCollider)) && attackBox.enabled)
        {
         
            playerInternalAction playerStats = col.gameObject.GetComponent<playerInternalAction>();
            if (playerStats.isDed())
            {
                playerDedDance = true;
                playerDed = true;
            }
            else
            {
                audioScript.makeAttackNoise();
                playerStats.applyDamage(myHitDamage);
            }
            
            

        }
        else if(col.gameObject.CompareTag("ally") && (col.GetType() == typeof(SphereCollider)) && attackBox.enabled)
        {
            audioScript.makeAttackNoise();
            npcInternalAction allyStats = col.gameObject.GetComponent<npcInternalAction>();
            allyStats.applyDamage(myHitDamage);

            
            if (allyStats.isDed())
            {
                foundAllyTarget = false;
                allyTarget = null;
                coinTossLost = false;

            }
        }
        else if(col.gameObject.CompareTag("beacon") && col.GetType() == typeof(BoxCollider))
        {
            beaconStats targetBeaconStats = col.gameObject.GetComponent<beaconStats>();
            if(targetBeaconStats.getIsPlayerCaptured())
            {
                audioScript.makeBeaconAttackNoise();
                targetBeaconStats.enemyCaptureAttack();
                if(!(targetBeaconStats.getIsPlayerCaptured()))
                {
                    artist.SetTrigger("victory");
                    capturedIntendedBeacon = true;
                    startVictoryTimer();

                }
            }


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

    void startVictoryTimer()
    {
        victoryTimer = 1.66f;
        victoryWiggle = true;
        playerDedDance = false;
    }

    public void killCharacter()
    {
        myStats.destroyCharacter(closestSpawnBox);
    }

    public void setSpawnPoint(GameObject spawnBox)
    {
        closestSpawnBox = spawnBox;
    }

    public void allyHasDied()
    {

        foundAllyTarget = false;
        allyTarget = null;

    }
}
