using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioplay : MonoBehaviour
{
    public AudioClip mouthSound;
    public AudioClip switchSound;
    private AudioSource audioSource;
    private int playCount = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        playCount++;
        if (playCount >= 3)
        {
            audioSource.clip = mouthSound;
            audioSource.volume = 0.08f;
            audioSource.Play();
        }
    }
    public void PlaySoundSwitch()
    {
        playCount++;
        if (playCount >= 2)
        {
            audioSource.clip = switchSound;
            audioSource.volume = 1.0f;
            audioSource.Play();
        }
    }

    void Update()
    {

    }
}
