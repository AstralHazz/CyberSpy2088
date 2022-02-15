using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int currentHealth;

    EnemyUICanvasController healthBar;
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponentInChildren<EnemyUICanvasController>();
        healthBar.SetMaxHealth(maxHealth);

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
