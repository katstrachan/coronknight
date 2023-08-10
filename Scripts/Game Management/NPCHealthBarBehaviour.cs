using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHealthBarBehaviour : MonoBehaviour
{
    Slider healthBar;
    // Start is called before the first frame update

    public void initialiseHealthBar(int maxHealth)
    {
        healthBar = gameObject.transform.Find("healthBar").GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }
    
    public void setHealthBar(int health)
    {
        healthBar.value = health;
    }
}
