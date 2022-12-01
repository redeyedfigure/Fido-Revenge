using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float energyBenefit = 2000;
    public PlayerAttributes attributes;

    //Only works if player is tagged as 'player'
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            attributes.health += energyBenefit;
            Destroy(gameObject);
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
