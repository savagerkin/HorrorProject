using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogDensity : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playerComponents; 
    [SerializeField] private Transform playerTransform;

    Color basefog; //S:35 V:52 to V:15 S:70 
    float baseDensity; //0.02

    [Header("Fog Settings")]
    [SerializeField] private bool change_fogColor;
    [SerializeField] private bool change_fogDensity;

    [Header("Fog Color Settings")]
    
    [SerializeField] private float initalS = 0.35f;
    [SerializeField] private float initalV = 0.52f;
    [SerializeField] private float finalS = 0.70f;
    [SerializeField] private float finalV = 0.15f;
    [SerializeField] private float MaximumHeight = 20;

    [Header("Fog Density Settings")]
    [SerializeField] private float targetfogDensity; //0.002f
    void Start()
    {
        basefog = RenderSettings.fogColor;
        Color.RGBToHSV(basefog, out float H, out _, out _);
        RenderSettings.fogColor = Color.HSVToRGB(H, initalS, initalV);
        baseDensity = RenderSettings.fogDensity;
        
    }

    // Update is called once per frame
    void Update()
    {
        float ypos = playerTransform.position.y - 40f ;
        float distance = Mathf.Abs(ypos - MaximumHeight);
        float normalizedDistance = Mathf.Clamp01(distance / MaximumHeight);
        if(change_fogColor)
        {
            setFogColor(normalizedDistance);
        }
        if(change_fogDensity)
        {
            setFogDensity(normalizedDistance);
        }
    }

    void setFogColor(float playerposY)
    {

        float newS = Mathf.Lerp(finalS,initalS, playerposY);
        float newV = Mathf.Lerp(finalV,initalV, playerposY);
        Color.RGBToHSV(basefog, out float H, out _, out _);
        Color newColor = Color.HSVToRGB(H, newS, newV);
        RenderSettings.fogColor = newColor;
        //Debug.Log("Fog Color: " + RenderSettings.fogColor);
    }

    void setFogDensity(float playerposY)
    {
        RenderSettings.fogDensity = Mathf.Lerp(targetfogDensity, 0.02f, playerposY);
        //Debug.Log("Fog Density: " + RenderSettings.fogDensity);
    }
}
