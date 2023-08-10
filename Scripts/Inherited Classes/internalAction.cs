using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class internalAction : MonoBehaviour
{
    protected float damageMultiplier;
    protected int damageDealt;
    protected int maxHealth;
    protected int health;
    protected Animator artist;
    float timer = 0;
    bool beingAttacked = false;

   


    private void Update()
    {
        if (timer  > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0f;
            beingAttacked = false;
        }
    }

    public void applyDamage(int hit)
    {
        
        health -= hit;
       
        artist.SetTrigger("hurting");
        if(gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<playerAudio>().makeHitNoise();
            gameObject.GetComponent<playerInternalAction>().changeHealthBar(health);
        }
        else if(gameObject.CompareTag("ally"))
        {
            gameObject.GetComponent<npcAudio>().makeHitNoise();
            gameObject.GetComponent<npcInternalAction>().changeHealthBar(health);
        }
        else if(gameObject.CompareTag("enemy"))
        {
            gameObject.GetComponent<npcAudio>().makeHitNoise();
        }

        setBeingAttacked();
        checkIfDed();

    }

    void setBeingAttacked()
    {
        timer = 3f;
        beingAttacked = true;

    }

    public bool getBeingAttacked()
    {
        return beingAttacked;
    }


    protected void checkIfDed()
    {
        bool ded = isDed();
        if (ded)
        {
            die();
        }
    }
    public bool isDed()
    {
        if(health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void die()
    {
        artist.SetTrigger("dying");
    }

   

    //Gets
    public float getDamageMultiplier()
    {
        return damageMultiplier;
    }
    public int getHealth()
    {
        return health;
    }
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public int getDamageDealt()
    {
        return damageDealt;
    }

    //Sets
    public void setDamageMultiplier(float i)
    {
        damageMultiplier = i;
    }
    public void setHealth(int i)
    {
        health = i;
    }
    public void setMaxHealth(int i)
    {
        maxHealth = i;
    }
    public void setDamageDealt(int i)
    {
        damageDealt = i;
    }
}
