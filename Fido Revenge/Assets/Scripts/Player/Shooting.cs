using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    /*
    Essential Aspects To Program:
    Weapon Switching
    */

    //----------------------------------------

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

    //Public Gun Stats
    [Header("Gun Stats")]
    public float gunDamage = 1;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public float weaponRecoil = 50f;
    public float fireRate = 0.25f;

    //Private Gun Stats
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    //Accessible GameObjects
    [Header("Game Objects")]
    public Transform gunEnd;
    public PlayerAttributes attributes;
    private new Camera camera;
    private AudioSource gunAudio;

    [Header("Bullet Limits and Reloading")]
    //The time it takes for the gun to reload
    public float reloadDuration = 2f;
    //The amount of energy required to reload the gun
    public float reloadEnergy = 3f;
    //The maximum amount of bullets that a reload can provide.
    public int bulletLimit = 6;
    //Amount of bullets that are currently within the gun
    private int currentBullets;
    //Shows if the player is reloading
    bool isReloading = false;

    //Related to an optional system of bullet accuracy
    [Header("Accuracy and Aiming")]
    //If enabled, a range of bullet accuracy is determined
    public bool accuracyEnabled = true;
    //The range of accuracy allowed for the player (if accuracyEnabled is enabled)
    public float bulletSpread = 0.5f;
    //The range of accuracy allowed for the player if the player is aiming
    public float aimedSpread = 0.3f;
    [System.NonSerialized]
    public bool isAiming = false;

    void Start()
    {
        //Garner access to AudioSource Component
        gunAudio = GetComponent<AudioSource>();

        //Garner access to Camera component;
        camera = GetComponentInParent<Camera>();

        currentBullets = bulletLimit;
    }

    void Update()
    {
        //If the player has ammo and meets the other terms, they can fire the gun.
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire && currentBullets > 0 && !isReloading)
        {
            Shoot();
        }

        //If the player doesn't have ammo, they can't fire the gun.
        else if (Input.GetButtonDown("Fire1") && Time.time > nextFire && currentBullets <= 0 && !isReloading)
        {
            Debug.Log ("Out of ammo, kid.");
        }

        //If the player presses 'R' and meets the other terms, they can reload
        if(Input.GetKeyDown(KeyCode.R) && currentBullets < bulletLimit && !isReloading && attributes.Reload(reloadEnergy))
        {
            Reload();
        }

        //If the player press 'R' but doesn't meet the other terms, they can't reload
        else if (Input.GetKeyDown(KeyCode.R) && currentBullets >= bulletLimit && !isReloading && attributes.Reload(reloadEnergy))
        {
            Debug.Log ("No need for that!");
        }

        //If the player doesn't have the energy to reload, they can't reload
        else if (Input.GetKeyDown(KeyCode.R) && attributes.Reload(reloadEnergy) == false)
        {
            Debug.Log ("Nice try.");
        }

        //Determines if the player is aiming
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
        nextFire = Time.time + fireRate;

        //Begins effects loop
        StartCoroutine (ShotEffect());

        //Create a vector at the center of camera's viewport
        Vector3 rayOrigin = camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

        //The direction of the raycast/bullet
        Vector3 rayDirection = camera.transform.forward;

        //Randomizes the direction of the bullet ray according to a bulletSpread float.
        if(accuracyEnabled && !isAiming)
        {
            rayDirection.x += Random.Range(-bulletSpread, bulletSpread);
            rayDirection.y += Random.Range(-bulletSpread, bulletSpread);
            rayDirection.z += Random.Range(-bulletSpread, bulletSpread);
        }
        //Aiming is more precise for those who are aiming...
        else if (accuracyEnabled && isAiming)
        {
            rayDirection.x += Random.Range(-aimedSpread, aimedSpread);
            rayDirection.y += Random.Range(-aimedSpread, aimedSpread);
            rayDirection.z += Random.Range(-aimedSpread, aimedSpread);
        }

        //Declare raycast hit
        RaycastHit hit;

        //Check if raycast has hit anything and applies damage / force accordingly
        if(Physics.Raycast (rayOrigin,rayDirection, out hit, weaponRange))
        {
            Shootable health = hit.collider.GetComponent<Shootable>();
            if (health != null)
            {
                health.Damage (gunDamage);
            }
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce (-hit.normal * hitForce);
            }
        }

        //Removes one bullet from current stockpile (because the bullet was, well, used)
        currentBullets = currentBullets - 1;
    }

    void Reload ()
    {
        Debug.Log("Reloading!");
        attributes.ReloadEffects(reloadEnergy);
        //Waits out the reloading time
        StartCoroutine (ReloadWaiting());
    }

    private IEnumerator ReloadWaiting()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        currentBullets = bulletLimit;
        isReloading = false;
    }

    private IEnumerator ShotEffect()
    {
        //Plays the sound effect
        gunAudio.Play();

        //Wait for .07 seconds
        yield return shotDuration;
    }
}
