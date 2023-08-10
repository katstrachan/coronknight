using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientationIconBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Quaternion playerDir;
    Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

   
    // Update is called once per frame
    public void makeRotate(Vector3 rAxis, float rAngle, Vector3 rOrigin)
    {
        transform.RotateAround(rOrigin, Vector3.forward, rAngle);
    }
}
