using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteract : MonoBehaviour
{
    //Allows player to interact with items and objects

    private SphereCollider pickUpArea;
    private playerInternalAction myStats;
    private Animator artist;
    private playerController player;
    private bool isHealing = false;

    void Start()
    {
        player = GetComponent<playerController>();
        artist = GetComponent<Animator>();
        pickUpArea = GetComponent<SphereCollider>();
        myStats = GetComponent<playerInternalAction>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("potion"))
        {
            
            GameObject potion = col.gameObject;
            myStats.heal(potion);
            
        }
    }

    void stopMoving()
    {
        isHealing = true;
        player.setCanMove(false);
    }

    void startMoving()
    {
        isHealing = false;
        player.setCanMove(true);
    }

    public bool getIsHealing()
    {
        return isHealing;
    }
}
