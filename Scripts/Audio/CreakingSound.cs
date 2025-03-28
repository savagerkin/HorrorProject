using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreakingSound : MonoBehaviour
{
    [SerializeField] AudioClip[] creakingClips;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Creak"))
        {
            AudioClip clip = GetRandomClip();
            audioSource.volume = 1f;
            audioSource.PlayOneShot(clip);
        }
    }
    
    private AudioClip GetRandomClip()
    {
        return creakingClips[UnityEngine.Random.Range(0, creakingClips.Length)];
    }
}
