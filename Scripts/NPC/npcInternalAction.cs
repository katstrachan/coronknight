using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcInternalAction : internalAction
{
    npcAudio audioScript;
    public ParticleSystem deathParticles;
    public ParticleSystem poofParticles;
    private float healthChance = 0f;
    private float healthChanceThreshold;
    public GameObject greenPotion;
    public GameObject redPotion;
    public GameObject bluePotion;
    NPCHealthBarBehaviour myHealthBar; //for allies only
    GameObject myGameManager;

    private void Awake()
    {
        damageDealt = 10;
        maxHealth = 100;
        health = maxHealth;
        
        

        if(gameObject.CompareTag("ally"))
        {
            myHealthBar = gameObject.transform.Find("NPCHealthbar").GetComponent<NPCHealthBarBehaviour>();
            myHealthBar.initialiseHealthBar(maxHealth);
        }
        
    }


    private void Start()
    {

        audioScript = gameObject.GetComponent<npcAudio>();
        artist = gameObject.GetComponent<Animator>();
        
    }  

    public void setGameManager(GameObject GM)
    {
        myGameManager = GM;
        gameManagerController gameManager = myGameManager.GetComponent<gameManagerController>();
        healthChanceThreshold = gameManager.getHealthChanceThreshold();
    }

    public void changeHealthBar(int health)
    {
        myHealthBar.setHealthBar(health);
    }



    public void destroyCharacter(GameObject mySpawn)
    {
        audioScript.makeDieNoise();
        
        Vector3 particlePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(poofParticles, transform.position, Quaternion.Euler(270f, 0f, 0f));
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        maybeSpawnHealth();

        if(mySpawn.CompareTag("beacon"))
        {
            mySpawn.GetComponent<beaconSpawnManager>().destroyDefender(gameObject);
        }
        else
        {
            mySpawn.GetComponent<boxSpawnManager>().destroySpawnee(gameObject);
        }
        
        Destroy(gameObject);
    }
    
    void maybeSpawnHealth()
    {
        float potionType = Random.Range(0f, 100f);
        GameObject potion;

        healthChance = Random.Range(0f, 10f);
        if(healthChance > healthChanceThreshold)
        {
            if(potionType <60)
            {
                potion = greenPotion;
            }
            else if(potionType >= 60 && potionType <85)
            {
                potion = redPotion;
            }
            else
            {
                potion = bluePotion;
            }
            Instantiate(potion, transform.position, Quaternion.identity);
        }
    }
 

}
