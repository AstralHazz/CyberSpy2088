using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    private GunSystem activeGun;
    public List<GunSystem> allGuns = new List<GunSystem>();
    public List<GunSystem> unlockableGuns = new List<GunSystem>();
    public int currentGunNumber;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GunSystem gun in allGuns)
        {
            gun.gameObject.SetActive(false);
        }

        activeGun = allGuns[currentGunNumber];
        activeGun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }
    }

    private void SwitchGun()
    {
        
        activeGun.gameObject.SetActive(false);
        currentGunNumber++;

        if (currentGunNumber >= allGuns.Count)
        {
            currentGunNumber = 0;
        }

        activeGun = allGuns[currentGunNumber];
        activeGun.gameObject.SetActive(true);
    }

    public void AddGun(string gunName)
    {
        bool unlocked = false;

        if(unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if(unlockableGuns[i].gunName == gunName)
                {
                    allGuns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                    unlocked = true;
                }
            }
        }

        if (unlocked)
        {
            currentGunNumber = allGuns.Count - 2;
            SwitchGun();
        }
    }
}
