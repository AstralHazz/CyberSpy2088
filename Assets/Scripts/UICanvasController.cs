using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICanvasController : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI totalAmmoText;

    public Slider healthSlider;
    public Slider mouseSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
}
