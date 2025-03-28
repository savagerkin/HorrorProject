using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PhoneRing : MonoBehaviour
{
    [SerializeField] private AudioClip ringSound;
    private bool oneTimeFlag = true;
    [SerializeField] private AudioClip heartBeatSound;
    [SerializeField] private AudioSource heartBeatAudioSource;

    [SerializeField] private AudioSource ringAudioSource;
    [SerializeField] private AudioSource windAudioSource;
    //[SerializeField] private AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        heartBeatAudioSource.volume = 0.3f;
        ringAudioSource.volume = 0.1f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && oneTimeFlag)
        {
            oneTimeFlag = false;
            playRingtone();
            PlayHeartBeatSound();
        }
    }

    void playRingtone()
    {
        ringAudioSource.clip = ringSound;
        ringAudioSource.loop = true;
        ringAudioSource.Play();
    }
    void PlayHeartBeatSound()
    {
        heartBeatAudioSource.clip = heartBeatSound;
        heartBeatAudioSource.loop = true;
        StartCoroutine(VolumeFade(windAudioSource, 2f, 0f));
        
        heartBeatAudioSource.Play();
    }

    /*
    IEnumerator FadeMixerGroupVolume(string exposedParam, float targetVolume, float duration)
    {
        float currentTime = 0;
        float currentVolume;
        audioMixer.GetFloat(exposedParam, out currentVolume);

        // Convert to linear scale (0-1 range for smooth interpolation)
        currentVolume = Mathf.Pow(10, currentVolume / 20);
        float targetVolumeLinear = Mathf.Pow(10, targetVolume / 20);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, targetVolumeLinear, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVolume) * 20);
            yield return null; // Wait until the next frame
        }

        // Ensure the target volume is set exactly at the end
        audioMixer.SetFloat(exposedParam, targetVolume);
    }

    */

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

    
}
