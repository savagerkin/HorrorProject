using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    private AudioSource audioSource; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        StartCoroutine(VolumeFade(audioSource, 2f, 0.20f));
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
        
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    // Smoothens volume change
    public IEnumerator VolumeFade(AudioSource source, float duration, float targetVolume)
    {
        float time = 0f;
        float starVol = source.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(starVol, targetVolume, time / duration);
            yield return null;
        }
        yield break;
    }
    
    // Smoothens pitch change
    public IEnumerator PitchFade(AudioSource source, float duration, float targetPitch)
    {
        float time = 0f;
        float startPitch = source.pitch;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.pitch = Mathf.Lerp(startPitch, targetPitch, time / duration);
            yield return null;
        }
        yield break;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Inside"))
        {
            StartCoroutine(VolumeFade(audioSource, 2f, 0.10f));
            StartCoroutine(PitchFade(audioSource, 2f, 0.6f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Inside"))
        {
            StartCoroutine(VolumeFade(audioSource, 2f, 0.20f));
            StartCoroutine(PitchFade(audioSource, 2f, 1f));
        }
    }
}
