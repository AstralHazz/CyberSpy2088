using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsPickup : MonoBehaviour
{
    public string gunToPickupName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponentInChildren<WeaponSwitchSystem>().AddGun(gunToPickupName);
            Destroy(gameObject);
        }
    }
}
