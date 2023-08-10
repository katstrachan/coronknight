using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patientMenu : MonoBehaviour
{

    float timer = 0f;
    float waitTime = 10f;
    Animator artist;
    // Start is called before the first frame update
    void Start()
    {
        artist = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>= waitTime)
        {
            timer = 0;
            artist.SetTrigger("laugh");
        }
        else
        {
            timerTick();
        }
    }

    void timerTick()
    {
        timer += Time.deltaTime;
    }
}
