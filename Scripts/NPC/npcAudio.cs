using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAudio : MonoBehaviour
{
    AudioSource myAudio;
    AudioClip attackNoise1;
    AudioClip attackNoise2;
    AudioClip attackNoise3;
    AudioClip dieNoise;
    AudioClip hitNoise;
    AudioClip beaconAttackNoise;
    GameObject gameManagerObj;
    gameManagerController gameManager;
    float vol;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        vol = PlayerPrefs.GetFloat("volume");
        myAudio.maxDistance = 50;
        myAudio.spatialBlend = 1;
        gameManagerObj = GameObject.FindGameObjectWithTag("gameManager");
        gameManager = gameManagerObj.GetComponent<gameManagerController>();
        getNoises();
    }

    void getNoises()
    {
        AudioClip[] clips = new AudioClip[6];
        if (gameObject.CompareTag("enemy"))
        {
            clips = gameManager.sendEnemyNoises();
            beaconAttackNoise = clips[3];
        }
        else if (gameObject.CompareTag("ally"))
        {
            clips = gameManager.sendAllyNoises();
            attackNoise2 = clips[3];
            attackNoise3 = clips[4];
        }

        attackNoise1 = clips[0];
        dieNoise = clips[1];
        hitNoise = clips[2];
        

    }

    // Update is called once per frame
    public void makeDieNoise()
    {
        
        AudioSource.PlayClipAtPoint(dieNoise, transform.position, vol );
    }
    
    public void makeAttackNoise()
    {
        if(gameObject.CompareTag("ally"))
        {
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                myAudio.PlayOneShot(attackNoise1);
            }
            else if (r == 1)
            {
                myAudio.PlayOneShot(attackNoise2);
            }
            else if (r == 2)
            {
                myAudio.PlayOneShot(attackNoise3);
            }
        }
        else
        {
            myAudio.PlayOneShot(attackNoise1);
        }
        
        
    }
    public void makeBeaconAttackNoise()
    {
        myAudio.PlayOneShot(beaconAttackNoise);
    }

    public void makeHitNoise()
    {
        myAudio.PlayOneShot(hitNoise);
    }
}
