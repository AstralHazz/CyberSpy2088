using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<GunSystem>().AddAmmo();

            AudioManager.instance.PlayerSFX(0);
            Destroy(gameObject);
        }
    }
}
