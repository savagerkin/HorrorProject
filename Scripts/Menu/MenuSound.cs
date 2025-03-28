using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    public void PlayButtonClickSound()
    {
        audioSource.volume = 0.3f;
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayButtonHoverSound()
    {
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(hoverSound);
    }
}
