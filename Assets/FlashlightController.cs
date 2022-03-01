using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    Light myLight;
    bool lightOn;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponentInChildren<Light>();
        myLight.intensity = 1.5f;
        lightOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            if (lightOn)
            {
                myLight.intensity = 0f;
                lightOn = false;
            }
            else if (!lightOn)
            {
                myLight.intensity = 1.5f;
                lightOn = true;
            }
    }
}