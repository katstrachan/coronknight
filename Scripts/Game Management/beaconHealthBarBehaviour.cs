using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class beaconHealthBarBehaviour : MonoBehaviour
{
    Slider healthBar;
    Image fill;
    Color allyColour = new Color(93f/255, 224f/255, 239f/255);
    Color enemyColour = new Color(95f/255, 190f/255, 127f/255);
    // Start is called before the first frame update
  
    public void initialiseHealthBar(int maxHealth, int minHealth, bool playerCaptured)
    {
        healthBar = gameObject.transform.Find("healthBar").GetComponent<Slider>();
        fill = healthBar.gameObject.transform.Find("fill").GetComponent<Image>();
        healthBar.maxValue = maxHealth;
        healthBar.minValue = minHealth;
        healthBar.value = maxHealth;
        setHealthBarColour(playerCaptured);
    }
    public void setHealthBar(int health)
    {
        healthBar.value = health;
    }

    public void setHealthBarColour(bool player)
    {
        if (player)
        {
            fill.color = allyColour;
        }
        else
        {
            fill.color = enemyColour;
        }
    }
}
