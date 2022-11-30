using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    /*
    Essential Aspects To Program:

    Recoil
    AccuracyRange
    BulletCount
    ReloadPeriod
    WeaponSwitching
    Left Click to shoot
    Right Click to Aim (increases accuracy, slows player speed)
    */

    //----------------------------------------

    /*
    Polishing:

    Screenshake
    Flare Effects
    Animations
    Audio Effects
    */

    //----------------------------------------

    /*
    Current Functionality:
    -States variable aspects that are currently unused.
    
    */

    //Public Gun Stats
    public float gunDamage = 1;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public float weaponRecoil = 50f;
    public float fireRate = 0.25f;

    //Private Gun Stats
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    //Accessible GameObjects
    public Transform gunEnd;
    private new Camera camera;
    private AudioSource gunAudio;

    void Start()
    {
        //Garner access to AudioSource Component
        gunAudio = GetComponent<AudioSource>();

        //Garner access to Camera component;
        camera = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            //Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            //Begins effects loop
            StartCoroutine (ShotEffect());

            //Create a vector at the center of camera's viewport
            Vector3 rayOrigin = camera.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            //Declare raycast hit
            RaycastHit hit;

            //Check if raycast has hit anything
            if(Physics.Raycast (rayOrigin,camera.transform.forward, out hit, weaponRange))
            {
                
            }

        }

    }

    private IEnumerator ShotEffect()
    {
        //Plays the sound effect
        gunAudio.Play();

        //Wait for .07 seconds
        yield return shotDuration;
    }
}
