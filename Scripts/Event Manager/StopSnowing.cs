using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSnowing : MonoBehaviour
{

    [SerializeField] private ParticleSystem snow;
 
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            snow.Stop();
        }
    }
    private void OnTriggerExit (Collider other)
    {
        if(other.CompareTag("Player"))
        {
            snow.Play();
        }
    }
}
