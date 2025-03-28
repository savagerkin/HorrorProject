using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlyingEffect : MonoBehaviour
{
    ParticleSystem birdParticles;   
    GameObject birdEffect;
    bool isFlying = false;
    // Start is called before the first frame update
    void Start()
    {
        birdParticles = GetComponentInChildren<ParticleSystem>();
        birdEffect = birdParticles.gameObject;
        birdParticles.Stop();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isFlying)
        {
            birdParticles.Play();
            Destroy(birdEffect,10f);
            isFlying = true;
        }
    }
}
