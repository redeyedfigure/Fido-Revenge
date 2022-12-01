using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponHandler : MonoBehaviour
{
    //List of every available weapon
    public GameObject[] weaponSet;
    //Defines the currently used weapon
    public GameObject currentWeapon;
    private int weaponChoice;
    //A bool that defines which weapons are currently available to the player
    public bool[] availableWeapons;

    void Start()
    {
        weaponChoice = 0;
        currentWeapon = weaponSet[0];
        availableWeapons = new bool[weaponSet.Length];
        availableWeapons[0] = true;
        availableWeapons[1] = true;
        availableWeapons[2] = true;
    }

    void Update()
    {
        currentWeapon = weaponSet[weaponChoice];

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSwitch(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSwitch(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponSwitch(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            WeaponSwitch(3);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            WeaponSwitch(4);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            WeaponSwitch(5);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            WeaponSwitch(6);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            WeaponSwitch(7);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            WeaponSwitch(8);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            WeaponSwitch(9);
        }
    }

    void WeaponSwitch (int weaponSwap)
    {
        if(availableWeapons[weaponSwap] == true && weaponSet[weaponSwap] != null)
        {
            weaponChoice = weaponSwap;
            weaponSet[weaponChoice].SetActive(true);
            for (int i = 0; i < weaponSet.Length; i++)
            {
                if(i != weaponChoice && weaponSet[i] != null)
                {
                    weaponSet[i].SetActive(false);
                }
            }
        }
    }
}
