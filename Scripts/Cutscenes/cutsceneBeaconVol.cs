using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneBeaconVol : MonoBehaviour
{
    AudioSource myAudio;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
    }

    
}
