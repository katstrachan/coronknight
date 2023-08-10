using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    // Start is called before the first frame update
    npcAudio audioScript;
    public NavMeshAgent enemy;
    public Transform player;
    public LayerMask playerLayer;
    private Rigidbody rb;
    public BoxCollider attackBox;
    private npcInternalAction myStats;
    private Animator artist;
    public ParticleSystem spawnParticles;
    private float followRadius = 5f;
    private float attackRadius = 2f;
    GameObject closestSpawnBox;
    
        

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

    //Debug
    public bool wanderingAllowed = false;
    public bool followingAllowed = false;
    public bool attackingAllowed = false;


    void Start()
    {
        audioScript = gameObject.GetComponent<npcAudio>();
        LayerMask spawnBoxLayer = LayerMask.GetMask("spawnBoxes");
        Instantiate(spawnParticles, transform.position, Quaternion.identity);
        //Get Components
        myStats = GetComponent<npcInternalAction>();
        attackBox = GetComponent<BoxCollider>();
        enemy = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        artist = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLayer = LayerMask.GetMask("player");
        myHitDamage = myStats.getDamageDealt();
        
        //Set-up
        xVar = 20;
        zVar = 20;
        attackBox.enabled = false;
        validPos = false;
        resetAttackCooldown();
        newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
     

        addAttackCooldownTime();
        animateMovement();

      
        bool playerSpotted = Physics.CheckSphere(transform.position, followRadius, playerLayer);
        bool playerInReach = Physics.CheckSphere(transform.position, attackRadius, playerLayer);



        
        if (player != null)
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
            resetAgent();
            wander();
        }
    }

  

    float findDistance(Transform otherObject)
    {
        return (transform.position - otherObject.position).sqrMagnitude;
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



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);

    }

    void followPlayer()
    {
        resetAttackCooldown();
        if (player != null)
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
        myStats.destroyCharacter(closestSpawnBox);
    }

    public void setSpawnPoint(GameObject spawnBox)
    {
        closestSpawnBox = spawnBox;
    }
}
