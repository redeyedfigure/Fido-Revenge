using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Gun Stats")]
    public float gunDamage = 1;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public float weaponRecoil = 50f;
    public float fireRate = 0.25f;
    //The amount of rays shot by the gun
    [Range(1,25)]
    public int shotCount = 1;
    //Determines if the weapon is an automatic weapon
    public bool automaticEnabled = false;

    [Header("Game Objects")]
    public AudioSource gunAudio;

    [Header("Spread & Aiming")]
    public float bulletSpread = 0.5f;
    public float aimedSpread = 0.3f;

    [Header("Reloading and Bullet Count")]
    //The time it takes for the gun to reload
    public float reloadDuration = 2f;
    //The amount of energy required to reload the gun
    public float reloadEnergy = 3f;
    //The maximum amount of bullets that a reload can provide.
    public int bulletLimit = 6;

    public int currentBulletCount;
}
