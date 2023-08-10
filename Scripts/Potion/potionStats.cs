using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionStats : MonoBehaviour
{
    public ParticleSystem pickUp;
    protected int pType;
    // Start is called before the first frame update

    public int largePotion = 100;
    public int medPotion = 50;
    public int smallPotion = 20;

    public void removeItem()
    {
        Instantiate(pickUp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public int getType()
    {
        return pType;
    }
    
}
