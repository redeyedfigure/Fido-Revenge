using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float health;
    public float armor;

    // This variable is just for use in making a massive health drop every 10 seconds. You can
    // remove this if you want, just make sure to remove lines 28-33 with it.
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
        // Heals armor by 2 points every second
        armor += 2 * Time.deltaTime;

        // making a massive health drop every ten seconds for testing puposes
        secondsSinceLastSpike += Time.deltaTime;
        if (secondsSinceLastSpike >= 10) {
            health -= 20;
            secondsSinceLastSpike -= 10;
        }
    }
}
