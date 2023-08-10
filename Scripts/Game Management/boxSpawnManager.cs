using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class boxSpawnManager : MonoBehaviour
{ 
    
 
    List<GameObject> enemies = new List<GameObject>();
    
    SphereCollider spawnRadiusSphere;
    float spawnRadius;
    public GameObject idleEnemy;
    public GameObject beaconHunterEnemy;
    float respawnTimer;
    float respawnTimerMax;
    bool respawnTimerEnabled = false;
    int idleEnemyLimit;
    int beaconHunterLimit;
    int boxLimit;
  
    float idleProb;
    float beaconHunterProb;

    // Start is called before the first frame update
    void Start()
    {
        spawnRadiusSphere = GetComponent<SphereCollider>();
        spawnRadius = spawnRadiusSphere.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnTimerEnabled)
        {
            respawnTimerTick();

        }
    }

    void respawnTimerTick()
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
        }
        else
        {
            respawnTimer = respawnTimerMax;

            if (enemies.Count != (boxLimit - 1))
            {
                spawnEnemies(1);
            }
           
           

            if (enemies.Count == (boxLimit - 1))
            {
                respawnTimerEnabled = false;
            }
        }


    }

    void startRespawnTimer()
    {
        respawnTimerEnabled = true;
    }

    public void setBoxEnemyQuantity(int idleEnemies, int beaconHunters, int boxEnemies)
    {
        idleEnemyLimit = idleEnemies;
        beaconHunterLimit = beaconHunters;
        boxLimit = boxEnemies;
    }

    public void setEnemyProbabilities(float idleEnemies, float beaconHunters)
    {
        idleProb = idleEnemies;
        beaconHunterProb = beaconHunters;
    }

    public void setRespawnInterval(float interval)
    {
        respawnTimerMax = interval;
        respawnTimer = respawnTimerMax;
    }

    public void initialiseSpawnBox()
    {
        spawnEnemies(boxLimit);
    }

    public void destroySpawnee(GameObject enemy)
    {
        enemies.Remove(enemy);
        startRespawnTimer();
    }

    void spawnEnemies(int quantity)
    {
        
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemyType = getEnemyType();
            Vector3 spawnPoint = findSpawnPoint();
            GameObject newEnemy = Instantiate(enemyType, spawnPoint, Quaternion.identity);
            if (enemyType == beaconHunterEnemy)
            {
                newEnemy.GetComponent<enemyBeaconHunterController>().setSpawnPoint(gameObject);
            }
            else
            {
                newEnemy.GetComponent<enemyController>().setSpawnPoint(gameObject);
            }
            
            enemies.Add(newEnemy);
        }

    }

    GameObject getEnemyType()
    {
        float r1 = Random.Range(0f, 10f);
        float r2 = Random.Range(0f, 10f);
        if( r1 > beaconHunterProb)
        {
            return beaconHunterEnemy;
        }
        else
        {
            return idleEnemy;
        }
    }

    Vector3 findSpawnPoint()
    {
        NavMeshHit navHit;

        bool validPos = false;
        Vector3 sp = new Vector3();

        while (validPos == false)
        {
            Vector3 point = transform.position + Random.insideUnitSphere * spawnRadius;
            bool pointFound = NavMesh.SamplePosition(point, out navHit, 1f, NavMesh.AllAreas);

            if (pointFound == true)
            {
                sp = navHit.position;
                validPos = true;
            }
        }
        return sp;
    }
}

