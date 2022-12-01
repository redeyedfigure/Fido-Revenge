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

    /*
    Current Functionality:
    -States variable aspects that are currently unused.
    -Left Click triggers a shoot function
    -Player can click R to reload, and it costs the player energy to reload a hand of bullets.
    -This energy is taken out of the PlayerAttributes script.
    -If enabled, then there is a range of accuracy for the bullet
    -There is also aiming functionality: press 'right click' to aim, and it both slows down the player
    (in the PlayerMovement script) as well as increases its accuracy.
    */

    //Private Gun Stats
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    //Accessible GameObjects
    [Header("Game Objects")]
    public PlayerAttributes attributes;
    public WeaponHandler handling;
    private new Camera camera;

    //Amount of bullets that are currently within the gun
    private int currentBullets;
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

        currentBullets = handling.currentWeapon.GetComponent<Weapon>().bulletLimit;
    }

    void Update()
    {
        //Accesses the stats of the currently used weapon
        weaponStats = handling.currentWeapon.GetComponent<Weapon>();

        //Checks if the player is able to fire the gun
        if(Time.time > nextFire && currentBullets > 0 && !isReloading)
        {
            ableToFire = true;
        }
        else
        {
            ableToFire = false;
        }

        //Checks if the player is able to reload the gun
        if(currentBullets < weaponStats.bulletLimit && !isReloading && attributes.Reload(weaponStats.reloadEnergy))
        {
            ableToReload = true;
        }
        else
        {
            ableToReload = false;
        }

        //To shoot or not to shoot
        if(Input.GetButtonDown("Fire1") && ableToFire)
        {
            Shoot();
        }
        else if(Input.GetButton("Fire1") && weaponStats.automaticEnabled)
        {
            Shoot();
        }
        else if (Input.GetButtonDown("Fire1") && !ableToFire)
        {
            Debug.Log ("Out of ammo, kid.");
        }

        //To reload or not to reload
        if(Input.GetKeyDown(KeyCode.R) && ableToReload)
        {
            Reload();
        }
        else if (Input.GetKeyDown(KeyCode.R) && !ableToReload)
        {
            Debug.Log ("No need for that!");
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

        //Begins effects loop
        StartCoroutine (ShotEffect());

        //Create a vector at the center of camera's viewport
        Vector3 rayOrigin = camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        //The direction of the raycast/bullet
        Vector3 rayDirection = camera.transform.forward;
        Vector3 alternativeRayDirection;

        RaycastHit hit;

        //If it is a perfectly accurate weapon that only fires one bullet per shot
        if(!weaponStats.accuracyEnabled && weaponStats.shotCount == 1)
        {
            if(Physics.Raycast(rayOrigin, rayDirection, out hit, weaponStats.weaponRange))
            {
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

        //If it is a not perfectly accurate weapon that fires one or more bullets per shot
        if(weaponStats.accuracyEnabled || weaponStats.shotCount > 1)
        {
            for (int i = 0; i < weaponStats.shotCount; i++)
            {
                alternativeRayDirection = rayDirection;
                if(isAiming)
                {
                    alternativeRayDirection.x += Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread);
                    alternativeRayDirection.y += Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread);
                    alternativeRayDirection.z += Random.Range(-weaponStats.aimedSpread, weaponStats.aimedSpread);
                }
                else if(!isAiming)
                {
                    alternativeRayDirection.x += Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
                    alternativeRayDirection.y += Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
                    alternativeRayDirection.z += Random.Range(-weaponStats.bulletSpread, weaponStats.bulletSpread);
                }
                if(Physics.Raycast(rayOrigin, rayDirection, out hit, weaponStats.weaponRange))
                {
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

        currentBullets = currentBullets - 1;
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
        currentBullets = weaponStats.bulletLimit;
        isReloading = false;
    }

    private IEnumerator ShotEffect()
    {
        //Plays the sound effect
        weaponStats.gunAudio.Play();

        //Wait for .07 seconds
        yield return shotDuration;
    }
}
