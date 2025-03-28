using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathSound : MonoBehaviour
{
    AudioSource audioSource; 
    [SerializeField] AudioClip[] breathSounds; 
    [SerializeField] ParticleSystem vaporParticles; 
    ParticleSystem.Particle[] particles; //array to hold the particles

    [SerializeField] private int particleThreshold = 100; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        particles = new ParticleSystem.Particle[vaporParticles.main.maxParticles];

        StartCoroutine(CheckParticles());
    }

    IEnumerator CheckParticles()
    {
        while(true)
        {
            // Wait for given seconds in code
            yield return new WaitForSeconds(3f);

            // Get the current number of particles
            if (particleThreshold >= 300)
            {
                audioSource.volume = 0.3f;
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(RandomBreathSound());
            }
        }
    }

    private AudioClip RandomBreathSound()
    {
        return breathSounds[Random.Range(0, breathSounds.Length)];
    }
}