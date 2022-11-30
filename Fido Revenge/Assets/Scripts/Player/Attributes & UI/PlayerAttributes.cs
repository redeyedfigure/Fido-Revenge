using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public double health;
    public float armor;

    //Visualization Necessities
    [System.NonSerialized]
    public string healthCount;
    [System.NonSerialized]
    public string armorCount;
    private double roundedHealth;
    private double roundedArmor;

    // This variable is just for use in making a massive health drop every 5 seconds. You can
    // remove this if you want.
    private float secondsSinceLastSpike = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrements player's health by half a point every second, meaning they have 200 seconds (~3 minutes) to complete the level assuming they don't take damage.
        health -= Time.deltaTime;
        // Heals armor by 2 points every second unless at max
        if(armor < 30) {
            armor += 2 * Time.deltaTime;
        }

        // damage health if the armor health is less than zero
        if (armor < 0) {
            health += armor;
            armor = 0;
        }

        // making a massive health drop every 5 seconds for testing puposes
        secondsSinceLastSpike += Time.deltaTime;
        if (secondsSinceLastSpike >= 2) {
            armor -= 20;
            secondsSinceLastSpike -= 2;
        }

        roundedHealth = System.Math.Round(health, 2);
        roundedArmor = System.Math.Round(armor, 1);
        healthCount = roundedHealth.ToString();
        armorCount = roundedArmor.ToString();

        if (health < 0) {
            
        }
    }

    public bool Reload(float energy)
    {
        if((health - energy) > 0)
        {
            return true;
            
        }
        else if ((health - energy) <= 0)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public void ReloadEffects(float energy)
    {
        health = health - energy;
    }
}
