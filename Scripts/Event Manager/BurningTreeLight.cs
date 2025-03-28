using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTreeLight : MonoBehaviour
{
    [SerializeField] private Transform player;
    GameObject destination; 
    ParticleSystem fire;
    Light burningLight;
    float distance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fire = GetComponent<ParticleSystem>();
        burningLight = GetComponent<Light>();
    }
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        burningLight.range = distance;
        if(distance <= 250f && distance >= 100f){
            float normalizedDistance = (distance - 100) / 150f;
            float decayFactor = Mathf.Exp(-normalizedDistance * 8f);
            burningLight.intensity = Mathf.Lerp(45f, 10f, decayFactor);
        }
        else if(distance < 100f){
            float normalizedDistance = distance / 100f;
            float decayFactor = Mathf.Exp(-normalizedDistance * 10f);
            burningLight.intensity = Mathf.Lerp(10f, 0f, decayFactor);
            
        }
    }
}
