using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allySwordBehaviour : MonoBehaviour
{
    int allyHitDamage;
    public GameObject myWielder;
    allyController myController;
    npcInternalAction allyStats;

    private void Start()
    {
        myController = myWielder.GetComponent<allyController>();
        allyStats = myWielder.GetComponent<npcInternalAction>();
        allyHitDamage = allyStats.getDamageDealt();

    }

    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("enemy"))
        {
          
            npcInternalAction enemyStats = col.gameObject.GetComponent<npcInternalAction>();
            enemyStats.applyDamage(allyHitDamage);
            bool isEnemyDed = enemyStats.isDed();
            if (isEnemyDed)
            {
                myController.enemyHasDied();
            }
        }


    }
}
