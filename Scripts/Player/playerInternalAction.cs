using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInternalAction : internalAction
{
    //Code that affects specifically the player's internal stats
    //Also extends internalAction to allow player to heal
    playerAudio audioScript;
    public ParticleSystem deathParticles;
    public ParticleSystem healParticles;
    public GameObject gameManagerObject;
    gameManagerController gameManager;
    UIManager UI;
    private void Awake()
    {
        damageDealt = 10;
        maxHealth = 100;
        health = maxHealth;
        gameManager = gameManagerObject.GetComponent<gameManagerController>();
        
    }
    private void Start()
    {
        audioScript = gameObject.GetComponent<playerAudio>();
        artist = gameObject.GetComponent<Animator>();
        UI = gameManagerObject.GetComponent<UIManager>();
        UI.initialisePlayerHealthBar(maxHealth);


    }
    public void heal(GameObject potion)
    {

        potionStats pStats = potion.GetComponent<potionStats>();
        int potionType = pStats.getType();
        audioScript.makeHealNoise();
        artist.SetTrigger("healing");
        health = capHealth(potionType);
        changeHealthBar(health);
        pStats.removeItem();
    }
    public void changeHealthBar(int Health)
    {
        UI.setPlayerHealthBar(health);
    }

    protected int capHealth(int healAmount)
    {
        int tempHealth = healAmount + health;
        if (tempHealth > maxHealth)
        {
            return maxHealth;
        }
        else
        {
            return tempHealth;
        }

    }


    public void destroyCharacter()
    {
        audioScript.makeDieNoise();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
        gameManager.lose();
    }

    void healing()
    {
        Instantiate(healParticles, transform.position, Quaternion.Euler(270,0,0));
    }

    public void killCharacter()
    {
        gameObject.GetComponent<playerAudio>().makeDieNoise();

        destroyCharacter();
    }
   
}
