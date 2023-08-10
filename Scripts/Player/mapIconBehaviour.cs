using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapIconBehaviour : MonoBehaviour
{
    Quaternion upright;
    // Start is called before the first frame update
   
    private void Awake()
    {
        upright = transform.rotation;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.rotation = upright;
    }
}
