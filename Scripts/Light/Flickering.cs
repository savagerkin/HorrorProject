using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{

    [SerializeField] private bool _isFlickering = false;
    [SerializeField] private float timeDelay;
    [SerializeField] private float minTime = 0.1f;
    [SerializeField] private float maxTime = 0.2f;
    [SerializeField] private float minFlick = 0.5f;
    [SerializeField] private float maxFlick = 0.8f; 
    [SerializeField] private float flick = 0.6f;

    [SerializeField] private Light pointLight;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(_isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        _isFlickering = true;
        timeDelay = Random.Range(minTime,maxTime);
        flick = Random.Range(minFlick,maxFlick);
        pointLight.intensity = flick;
        yield return new WaitForSeconds(timeDelay);
        timeDelay = Random.Range(minTime,maxTime);
        flick = Random.Range(minFlick,maxFlick);
        pointLight.intensity = flick;
        yield return new WaitForSeconds(timeDelay);
        _isFlickering = false;
    }
}