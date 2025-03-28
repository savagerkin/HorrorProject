using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundrie : MonoBehaviour
{

    [SerializeField] GameObject screenEffect;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Entered collider " + other.gameObject.name);
            screenEffect.SetActive(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Exit the collider " + other.gameObject.name);
            screenEffect.SetActive(true);
        }
    
    }
}
