using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordBehaviour : MonoBehaviour
    //Checks for collsions with sword
    //Invokes damage on enemy from collision
{
    
    int playerHitDamage;
    public GameObject myWielder;
    playerInternalAction playerStats; 

    private void Start()
    {    
        playerStats = myWielder.GetComponent<playerInternalAction>();
        playerHitDamage = playerStats.getDamageDealt();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
           
            npcInternalAction enemyStats = col.gameObject.GetComponent<npcInternalAction>();
            enemyStats.applyDamage(playerHitDamage);
            

        }
        else if (col.gameObject.CompareTag("beacon"))
        {
            beaconStats targetBeaconStats = col.gameObject.GetComponent<beaconStats>();
            if (!targetBeaconStats.getIsPlayerCaptured())
            {
                targetBeaconStats.playerCaptureAttack();
            }
        }
    }
}
