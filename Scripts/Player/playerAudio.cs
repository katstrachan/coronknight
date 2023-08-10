using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudio : MonoBehaviour
{

    AudioSource myAudio;
    public AudioClip attackNoise;
    public AudioClip hitNoise;
    public AudioClip shieldBlockNoise;
    public GameObject footstepObj;
    AudioSource footsteps;
    public AudioClip dieNoise;
    public AudioClip healNoise;
    
    float vol;
    // Start is called before the first frame update

    private void Start()
    {
        footsteps = footstepObj.GetComponent<AudioSource>();
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        footsteps.volume = PlayerPrefs.GetFloat("volume");
        vol = PlayerPrefs.GetFloat("volume");
    }

    public void makeDieNoise()
    {
        AudioSource.PlayClipAtPoint(dieNoise, transform.position, vol);
    }

    public void makeHealNoise()
    {
        myAudio.PlayOneShot(healNoise);
    }

    public void makeAttackNoise()
    {
        myAudio.PlayOneShot(attackNoise);
    }

    public void makeHitNoise()
    {
        myAudio.PlayOneShot(hitNoise);
    }

    public void makeShieldBlockNoise()
    {
        myAudio.PlayOneShot(hitNoise);
    }

    public void startFootsteps()
    {
        if(!footsteps.isPlaying)
        {
            footsteps.Play();
        }
        
    }

    public void endFootsteps()
    {
        footsteps.Pause();
    }
}
