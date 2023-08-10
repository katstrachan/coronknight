using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class beaconSpawnManager : MonoBehaviour
{
    beaconStats myBeaconStats;
    bool playerCaptured;
    List<GameObject> defenders = new List<GameObject>();
    int allyLimit;
    int enemyLimit;
    SphereCollider beaconRadiusSphere;
    float beaconRadius;
    public GameObject ally;
    public GameObject enemy;
    public GameObject myGameManager;
    float respawnTimer;
    float respawnTimerMax;
    bool respawnTimerEnabled = false;
    int beaconLimit;

    // Start is called before the first frame update
    void Start()
    {
        myBeaconStats = GetComponent<beaconStats>();
        playerCaptured = myBeaconStats.getIsPlayerCaptured();
        beaconRadiusSphere = GameObject.Find("beaconAOI").GetComponent<SphereCollider>();
        beaconRadius = beaconRadiusSphere.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if(respawnTimerEnabled)
        {
            respawnTimerTick();
            
        }
    }

    void respawnTimerTick()
    {
        if(respawnTimer >0)
        {
            respawnTimer -= Time.deltaTime;
        }
        else
        {
            respawnTimer = respawnTimerMax;

            playerCaptured = myBeaconStats.getIsPlayerCaptured();
            if (playerCaptured)
            {
                if (defenders.Count < beaconLimit)
                {
                    spawnDefenders(ally, 1);
                }
            }
            else if(!playerCaptured)
            {
                if (defenders.Count <= beaconLimit)
                {
                    spawnDefenders(enemy, 1);
                }
                
            }

            if(defenders.Count > beaconLimit)
            {
                respawnTimerEnabled = false;
            }
        }


    }

   

    void startRespawnTimer()
    {
        respawnTimerEnabled = true;
    }

    public void setDefenderQuantity(int allies, int enemies)
    {
        allyLimit = allies;
        enemyLimit = enemies;
    }
    public void setRespawnInterval(float interval)
    {
        respawnTimerMax = interval;
        respawnTimer = respawnTimerMax;
    }

    public void beaconResetCapture(bool playerCaptured)
    {
        if(defenders.Count != 0)
        {
            destroyCurrentDefenders();
        }
        
        
        if(playerCaptured)
        {
            spawnDefenders(ally, allyLimit);
            beaconLimit = allyLimit;
        }
        else
        {
            spawnDefenders(enemy, enemyLimit);
            beaconLimit = enemyLimit;
        }
           
            
        
        
    }
    public void destroyDefender(GameObject defender)
    {
        defenders.Remove(defender);
        startRespawnTimer();
    }

    void destroyCurrentDefenders()
    {       
        int defendersPrevLength = defenders.Count;
        
        for(int i = 0; i < defendersPrevLength; i++)
        {
            defenders[0].GetComponent<npcInternalAction>().destroyCharacter(gameObject);
        }
        defenders.Clear();
    }

    void spawnDefenders(GameObject defenderType, int quantity)
    {
        
        for(int i = 0; i<quantity; i++)
        {
            Vector3 spawnPoint = findSpawnPoint();
            GameObject newDefender = Instantiate(defenderType, spawnPoint, Quaternion.identity);
            newDefender.GetComponent<npcInternalAction>().setGameManager(myGameManager);
            defenders.Add(newDefender);
            if (defenderType == ally)
            {
                newDefender.GetComponent<allyController>().setSpawnPoint(gameObject);
            }
            else
            {
                newDefender.GetComponent<baseEnemyController>().setSpawnPoint(gameObject);
            }
        }
            
    }

    Vector3 findSpawnPoint()
    {
        NavMeshHit navHit;
        
        bool validPos = false;
        Vector3 sp = new Vector3();

        while (validPos == false)
        {
            Vector3 point = transform.position + Random.insideUnitSphere * beaconRadius;
            bool pointFound = NavMesh.SamplePosition(point, out navHit, 1f, NavMesh.AllAreas);

            if(pointFound == true)
            {
                sp = navHit.position;
                validPos = true;
            }
        }
        return sp;
    }

    
 
}
