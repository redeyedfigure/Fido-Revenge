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
    private Camera camera;
    private Transform gunEnd;
    private AudioSource gunAudio;
    private LineRenderer laserLine;

    void Start()
    {
        //Garner access to LineRenderer Component
        laserLine = GetComponent<LineRenderer>();

        //Garner access to AudioSource Component
        gunAudio = GetComponent<AudioSource>();

        //Garner access to Camera component;
        camera = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
        }
    }
}
