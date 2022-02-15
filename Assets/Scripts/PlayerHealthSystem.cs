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

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlayerSFX(3);

            gameObject.SetActive(false);

            AudioManager.instance.StopBackgroundMusic();

            Instantiate(deathEffect, transform.position, transform.localRotation);
            FindObjectOfType<GameManager>().PlayerRespawn();
        }

        AudioManager.instance.PlayerSFX(4);
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
