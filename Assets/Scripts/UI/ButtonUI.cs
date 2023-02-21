using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    public AudioClip[] buttonHoverSounds;
    public AudioClip[] buttonClickSounds;

    public AudioSource audSource;

    public void PlayHoverSound()
    {
        int num = Random.Range(0, buttonHoverSounds.Length);

        audSource.clip = buttonHoverSounds[num];
        audSource.Play();
    }

    public void PlayClickSound()
    {
        int num = Random.Range(0, buttonClickSounds.Length);

        audSource.clip = buttonClickSounds[num];
        audSource.Play();
    }
}
