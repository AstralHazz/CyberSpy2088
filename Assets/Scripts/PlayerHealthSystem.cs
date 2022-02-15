using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth = 10;
    public GameObject deathEffect;
    UICanvasController healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar = FindObjectOfType<UICanvasController>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amountOfDamage)
    {
        currentHealth -= amountOfDamage;

        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Instantiate(deathEffect, transform.position, transform.localRotation);
            FindObjectOfType<GameManager>().PlayerRespawn();
        }
    }

    public void HealPlayer(int healFactor)
    {
        currentHealth += healFactor;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(maxHealth);
        }
        healthBar.SetHealth(currentHealth);
    }
}
