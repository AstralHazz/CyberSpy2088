using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    Rigidbody myRigidBody;
    public float upForce, forwardForce, grenadeLife;
    public int amountOfDamage = 5;
    public GameObject detonationEffect;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();

        GrenadeThrow();
    }

    private void GrenadeThrow()
    {
        myRigidBody.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
        myRigidBody.AddForce(transform.up * upForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        grenadeLife -= Time.deltaTime;
        if (grenadeLife < 0)
        {
            Instantiate(detonationEffect, transform.position, transform.localRotation);
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealthSystem>().TakeDamage(amountOfDamage);
            Instantiate(detonationEffect, transform.position, transform.localRotation);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(detonationEffect, transform.position, transform.localRotation);
            Destroy(gameObject);
        }
    }
}
