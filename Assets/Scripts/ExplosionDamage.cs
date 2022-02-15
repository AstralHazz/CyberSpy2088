using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public int explosionDamage;
    public bool trail;

    private void Start()
    {
        StartCoroutine(DestroyObject());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealthSystem>().TakeDamage(explosionDamage);
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealthSystem>().TakeDamage(explosionDamage);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
