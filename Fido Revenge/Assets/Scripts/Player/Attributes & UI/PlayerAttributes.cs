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
    private double roundedHealth;

    // This variable is just for use in making a massive health drop every 5 seconds. You can
    // remove this if you want, just make sure to remove lines 37-40 with it.
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
        if (secondsSinceLastSpike >= 5) {
            armor -= 20;
            secondsSinceLastSpike -= 5;
        }

        roundedHealth = System.Math.Round(health, 2);
        healthCount = roundedHealth.ToString();
    }
}
