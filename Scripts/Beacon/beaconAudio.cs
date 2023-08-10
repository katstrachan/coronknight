using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beaconAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource myAudio;
    public AudioClip enemyCaptureNoise;
    public AudioClip playerCaptureNoise;
    public AudioClip hitNoise; // maybe

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
    }

    // Start is called before the first frame update

    public void makeEnemyCaptureNoise()
    {
        myAudio.PlayOneShot(enemyCaptureNoise);
    }

    public void makeAllyCaptureNoise()
    {
        myAudio.PlayOneShot(enemyCaptureNoise);
    }

    
}
