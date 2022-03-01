using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGM;
    public AudioSource BGM2;

    public AudioSource[] SFXs;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void PlayerSFX(int sfxNumber)
    {
        SFXs[sfxNumber].Stop();
        SFXs[sfxNumber].Play();

    }

    public void StopBackgroundMusic()
    {
        BGM.Stop();
        BGM2.Stop();
    }
}
