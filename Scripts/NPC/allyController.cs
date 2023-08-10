using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class allyController : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent ally;
    public Transform enemy;
    public LayerMask enemyLayer;
    public LayerMask beaconLayer;
    private Rigidbody rb;
    public CapsuleCollider sword;
    private npcInternalAction myStats;
    private Animator artist;
    private GameObject closestBeacon;
    private beaconStats myBeacon;
    private SphereCollider myBeaconRadiusSphere;
    private float myBeaconRadius;
    private GameObject myWeapon; 
    private float followRadius = 20f;
    private float attackRadius = 2f;
    
    npcAudio audioScript;


    //Wander Vars
    private Vector3 wanderToPos = Vector3.zero;
    private bool validPos;
    bool posFound;
    public Vector3 newPos;
 
 
    float waitTimer = 5f;


    public Vector3 knockBackForce = new Vector3(0f, 0f, 1f);

    //Attack Vars
    int myHitDamage;
    public float attackWaitTime = 1f;
    float attackCooldown;
    public bool enemyFound = false;
   
    //Debug
    public bool wanderingAllowed = true;
    public bool followingAllowed = true;
    public bool attackingAllowed = true;

    public ParticleSystem spawnParticles;

    void Start()
    {
        
        audioScript = GetComponent<npcAudio>();
        Instantiate(spawnParticles, transform.position, Quaternion.identity);
        ally = GetComponent<NavMeshAgent>();

        setWaitTimer();
        myWeapon = transform.Find("root/pelvis/Weapon/sw03").gameObject;
        myStats = GetComponent<npcInternalAction>();
        sword = myWeapon.GetComponent<CapsuleCollider>();
        
        rb = GetComponent<Rigidbody>();
        artist = GetComponent<Animator>();

        //Masks
        enemyLayer = LayerMask.GetMask("enemy");
        beaconLayer = LayerMask.GetMask("pillar");
        

        //Set-up
       
        sword.enabled = false;
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
        bool enemySpotted = Physics.CheckSphere(transform.position, followRadius, enemyLayer);
        bool enemyInReach = Physics.CheckSphere(transform.position, attackRadius, enemyLayer);
        bool enemyWithinBeacon = Physics.CheckSphere(closestBeacon.transform.position, myBeaconRadius, enemyLayer);


        //Attack enemy within beacon range and our sight
        if (enemySpotted && enemyInReach && attackingAllowed && enemyWithinBeacon)
        {

            //if we don't already have a target
            if (!enemyFound)
            {
                findEnemyInRange(false);
                //if enemy that is not already engaged, attack
                if (enemyFound == true)
                {
                    launchAttack();
                    
                }
                else
                {
                    
                    resetAgent();
                    wander();
                }
            }
            else
            {
                launchAttack();
            }
        }
            //Follow enemy in sight and beacon range
        else if (enemySpotted && !enemyInReach && followingAllowed && enemyWithinBeacon)
        {
                
               
                //if we dont habe a target already
            if (!enemyFound)
            {
                findEnemyInRange(true);
                if (enemyFound == true)
                {
                    launchFollow();
                }
                else
                {
                   
                    wander();
                }
            }
            else
            {
                launchFollow();
            }
                
        }
            //Wander
        else if (wanderingAllowed)
        {
          
            enemyFound = false;
            wander();
        }
    }
       
    


    void launchFollow()
    {
        artist.SetTrigger("alert");
        transform.LookAt(enemy);
        followEnemy();
    }
    
    void launchAttack()
    {
        transform.LookAt(enemy);
        resetAgent();
        if (attackCooldown > attackWaitTime)
        {
            audioScript.makeAttackNoise();
            intendAttack();
        }
    }

    void findEnemyInRange(bool isFollowing) 
    {
        //set up
        bool underAttack;
        Collider[] enemies;

        //find all enemy objects in range
        if (isFollowing)
        {
            enemies = Physics.OverlapSphere(transform.position, followRadius, enemyLayer);
           
        }
        else
        {
            enemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        }
       


        // if only 1
        if (enemies.Length == 1)
        {
            underAttack = enemies[0].gameObject.GetComponent<internalAction>().getBeingAttacked();
            

            //if enemy isnt already being attacked
            if (!underAttack)
            {
                //weve found our enemy
                enemy = enemies[0].gameObject.transform;
                enemyFound = true;
            }
        }
        //if multiple enemies
        else if(enemies.Length != 0)
        {

            float d = 10000f;
            Transform closest = enemies[0].gameObject.transform;

            for(int i = 0; i<enemies.Length ; i++)
            {
                underAttack = enemies[i].gameObject.GetComponent<internalAction>().getBeingAttacked();
                float nextD = findDistance(enemies[i].gameObject.transform);
                
                if(!underAttack && (nextD <= d))
                {
                    closest = enemies[i].gameObject.transform;
                    d = nextD;
                    enemyFound = true;
                }
            }
            //enemyFound remains false if all enemies are engaged    

            enemy = closest;
        }



    }
    
    public bool checkIfAttackingEnemy()
    {
        if(enemyFound)
        {
            return true;
        }
        else
        {
            return false;
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
        if (ally.velocity.x != 0f && ally.velocity.z != 0f)
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
        enemyFound = false;
        enemy = null;
       
       
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
               
                    ally.SetDestination(wanderToPos);
               
            }
        }

    }


    void waitTimerTick()
    {
        if(waitTimer >0f)
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


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
      
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        
    }

    float distance(Vector3 a, Vector3 b) 
    {
        return (a - b).magnitude;
    }


    Vector3 findNewPos() 
    {
       
        NavMeshHit navHit;
        RaycastHit hit;

        bool validPos = false;
        Vector3 sp = new Vector3();

        while (validPos == false)
        {
            Vector3 point = closestBeacon.transform.position + Random.insideUnitSphere *(2* myBeaconRadius);
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
   

    void followEnemy()
    {
        resetAttackCooldown();
        if (enemy != null)
        {
            transform.LookAt(enemy);
            Vector3 enemyPos = new Vector3(enemy.position.x, enemy.position.y, enemy.position.z);
            ally.SetDestination(enemyPos);
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
        int swingType = randomiseAnimation();
        artist.SetInteger("swing", swingType);
        
        artist.SetTrigger("attacking");


    }

    int randomiseAnimation()
    {
        int r = Random.Range(0, 3);
        return r;
    }

    void resetAttackCooldown()
    {
        attackCooldown = attackWaitTime + 1;
    }

    void attackAlert()
    {
        sword.enabled = !sword.enabled;
    }

  

    void resetAgent()
    {
        ally.enabled = false;
        ally.enabled = true;
    }


    public void enemyHasDied()
    {
        
        enemyFound = false;
        enemy = null;
       
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
