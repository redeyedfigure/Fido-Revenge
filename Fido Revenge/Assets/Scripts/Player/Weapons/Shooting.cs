using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    /*
    Polishing:

    Recoil
    Screenshake
    Flare Effects
    Animations
    Audio Effects
    */

    //----------------------------------------

    //Private Gun Stats
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    //Accessible GameObjects
    [Header("Game Objects")]
    public PlayerAttributes attributes;
    public WeaponHandler handling;
    public GameObject impactEffect;
    private new Camera camera;

    //Shows if the player is reloading
    bool isReloading = false;

    //Ability Checks
    [System.NonSerialized]
    public bool isAiming = false;
    bool ableToFire = false;
    bool ableToReload = false;

    //Currently used weapon
    Weapon weaponStats;

    void Start()
    {
        //Garner access to Camera component;
        camera = GetComponentInParent<Camera>();
        StartCoroutine(BulletSet());
    }

    void Update()
    {
        //Accesses the stats of the currently used weapon
        weaponStats = handling.currentWeapon.GetComponent<Weapon>();

        //Checks if the player is able to fire the gun
        if(Time.time > nextFire && weaponStats.currentBulletCount > 0 && !isReloading)
        {
            ableToFire = true;
        }
        else
        {
            ableToFire = false;
        }

        //Checks if the player is able to reload the gun
        if(weaponStats.currentBulletCount < weaponStats.bulletLimit && !isReloading && attributes.Reload(weaponStats.reloadEnergy))
        {
            ableToReload = true;
        }
        else
        {
            ableToReload = false;
        }

        //To shoot or not to shoot
        if(Input.GetButtonDown("Fire1") && ableToFire && !weaponStats.automaticEnabled)
        {
            Shoot();
        }
        else if(Input.GetButton("Fire1") && ableToFire && weaponStats.automaticEnabled)
        {
            Shoot();
        }

        //To reload or not to reload
        if(Input.GetKeyDown(KeyCode.R) && ableToReload)
        {
            Reload();
        }
        else if(Input.GetKeyDown(KeyCode.R) && !ableToReload)
        {
            Debug.Log("Can't Reload!");
        }

        //To aim or not to aim
        if(Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

    }

    void Shoot ()
    {
        //Update the time when our player can fire next
        nextFire = Time.time + weaponStats.fireRate;
        weaponStats.currentBulletCount = weaponStats.currentBulletCount - 1;
        StartCoroutine (ShotEffect());

        //Create a vector at the center of camera's viewport
        Vector3 rayOrigin = camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        //The direction of the raycast/bullet
        Vector3 alternativeRayDirection;

        RaycastHit hit;

        for (int i = 0; i < weaponStats.shotCount; i++)
        {
            alternativeRayDirection = camera.transform.forward;
            if(isAiming)
            {
                alternativeRayDirection += new Vector3(Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread), 
                Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread), 
                Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread));
            }
            else if(!isAiming)
            {
                alternativeRayDirection += new Vector3(Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread), 
                Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread), 
                Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread));
            }
            if(Physics.Raycast(rayOrigin, alternativeRayDirection, out hit, weaponStats.weaponRange))
            {
                GameObject ImpactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Shootable health = hit.collider.GetComponent<Shootable>();
                if (health != null)
                {
                    health.Damage (weaponStats.gunDamage);
                }
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce (-hit.normal * weaponStats.hitForce);
                }
            }
        }
    }

    void Reload ()
    {
        Debug.Log("Reloading!");
        attributes.ReloadEffects(weaponStats.reloadEnergy);
        //Waits out the reloading time
        StartCoroutine (ReloadWaiting());
    }

    private IEnumerator ReloadWaiting()
    {
        isReloading = true;
        yield return new WaitForSeconds(weaponStats.reloadDuration);
        weaponStats.currentBulletCount = weaponStats.bulletLimit;
        isReloading = false;
    }

    private IEnumerator ShotEffect()
    {
        //Plays the sound effect
        weaponStats.gunAudio.Play();

        //Wait for .07 seconds
        yield return shotDuration;
    }

    private IEnumerator BulletSet()
    {
        yield return new WaitForSeconds(0.1f);
        weaponStats.currentBulletCount = weaponStats.bulletLimit;
    }
}
