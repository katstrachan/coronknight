using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    //Script for attack and defence of player character 
    //Interacts with the sword and shield colliders
    playerAudio audioScript;
    public GameObject myWeapon;
    public GameObject myShield;
    private CapsuleCollider swordCol;
    private BoxCollider shieldCol;
    private SphereCollider myBodyCol;
    private Animator artist;
    private playerController myMovement;
    public ParticleSystem shieldParticles;
    public playerInteract interact;
    bool makeShieldNoise = true;

    bool notMoving;
    bool grounded;
    bool healing;
    bool def = false;

    public void Start()
    {
        interact = GetComponent<playerInteract>();
        artist = GetComponent<Animator>();
        myMovement = GetComponent<playerController>();
        swordCol = myWeapon.GetComponent<CapsuleCollider>();
        myBodyCol = GetComponent<SphereCollider>();
        shieldCol = GetComponent<BoxCollider>();
        audioScript = GetComponent<playerAudio>();
        swordCol.enabled = false;
        shieldCol.enabled = false;    
    }

    
    private void Update()
    {

        healing = interact.getIsHealing();
        notMoving = myMovement.getIfCanDefend();
        grounded = myMovement.getIfGroundedPlayer();
        //Can defend check
        if ((notMoving && grounded && (healing != true)))
        {
            //Defence
            def = Input.GetButton("Fire2");
            makeDefend(def);
        }
        else
        {
            def = false;
        }
        
        if(!def && !healing)
        {
            //Attack
            bool attack = Input.GetButtonDown("Fire1");
            if (attack)
            {
                audioScript.makeAttackNoise();
                makeAttack();
            }
        }
       
        
        
       
    }

    void makeDefend(bool defend)
    {
        if (defend)
        {

            //Stop movement, enable collider, animation
            myMovement.setCanMove(false);
            shieldCol.enabled = true;
            myBodyCol.enabled = false;
            makeShieldNoiseCheck();
            artist.SetBool("defending", true);
        }
        else
        {
            myMovement.setCanMove(true);
            shieldCol.enabled = false;
            myBodyCol.enabled = true;
            artist.SetBool("defending", false);
            makeShieldNoise = true;
        }
    }

    void makeShieldNoiseCheck()
    {
        if(makeShieldNoise)
        {
            audioScript.makeShieldBlockNoise();
            makeShieldNoise = false;
        }
    }

    void makeAttack()
    {
        int swingType = randomiseAnimation();
        artist.SetInteger("swing", swingType);
        
        artist.SetTrigger("attacking");
        
    }

    void attackAlert()
    {
        
        changeColliderActivity(swordCol);
    }

    int randomiseAnimation()
    {
        
        int r = Random.Range(0, 3);
        return r;
    }

    public void changeColliderActivity(Collider col)
    {
        
        col.enabled = !col.enabled;
          
    }
    public void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.CompareTag("enemy") && (shieldCol.enabled == true) && (col.GetType() == typeof(BoxCollider)))
        {
            Instantiate(shieldParticles, transform.position, Quaternion.identity);
            artist.SetTrigger("shieldHit");

        }

    }


}


    

    

