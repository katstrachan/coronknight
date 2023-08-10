using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuDogAnimations : MonoBehaviour
{
    public Animator artist;
    float wait = 10;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        artist.SetTrigger("move");
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            artist.SetTrigger("move");
            resetTimer();
        }
        else
        {
            timerTick();
        }
    }

    void timerTick()
    {
        timer -= Time.deltaTime;
    }
    void resetTimer()
    {
        timer = wait;
    }
}
