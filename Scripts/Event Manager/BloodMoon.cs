using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BloodMoon : MonoBehaviour
{
    /*
    [MenuItem("Example/Create Lightning Settings")]
    static void CreateExampleLightingSettings()
    {
        LightingSettings lightingSettings = new LightingSettings();
        lightingSettings.albedoBoost = 8.0f;
        Lightmapping.lightingSettings = lightingSettings;
    }
    */
    [SerializeField] GameObject bloodCandlePrefab;
    [SerializeField] GameObject moon;
    [SerializeField] GameObject bloodMoon;
    [SerializeField] Material skybox;
    [SerializeField] Light sunLight;
    bool bloodEffect = false;
    bool annen = false;
    bool normalSceneEffect = true;
    Color fogcolor = new Color(0.0745098f, 0.0352941f, 0.0392157f);
    float time = 0.0f;

    public LightingSettings newLightingSettings;

    GameObject[] candles = new GameObject[6];
    GameObject[] originalCandles = new GameObject[6];
    GameObject[] bloodCandles = new GameObject[6];
    void Start()
    {
        moon.SetActive(true);
        if(bloodMoon!= null)
        {
            bloodMoon.SetActive(false);
        }
        Transform[] children = GetComponentsInChildren<Transform>();
        int index = 1;
        foreach (Transform child in children)
        {
            // Do something with each child transform
            if(child.name == "Candle " + index)
            {
                candles[index-1] = child.gameObject;
                originalCandles[index-1] = child.gameObject;
                Debug.Log("Child: " + child.name);
                Debug.Log(annen);
                child.gameObject.SetActive(false);
                index++;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("bool: " + _candles);
        time += Time.deltaTime;
        if(time < 5.0)
        {   
            if(normalSceneEffect)
            {
                normalScene();
                
                moon.SetActive(true);
                bloodMoon.SetActive(false);
                bloodEffect = true;
                normalSceneEffect = false;
                
            }
            
        }
        else if(time >= 5.0 && time < 10.0)
        {
            if(bloodEffect)
            {
            bloodMoonEffect();
            moon.SetActive(false);
            bloodMoon.SetActive(true);
            bloodEffect = false;
            normalSceneEffect = true;
            }
        }
        else if(time >= 10.0)
        {
            time = 0.0f;
        }
        //Debug.Log("Time: " + time);
    }

    void bloodMoonEffect()
    {
        
        int index = 1;
        //Debug.Log("Blood Moon Effect");
        if(sunLight != null)
        {
            sunLight.intensity = 0.12f;
            sunLight.color = new Color(1, 0, 0);
            //bloodMoon.SetActive(true);
        }
        if(skybox != null)
        {
            skybox.SetColor("_Tint", new Color(0.02352941f, 0.02745098f, 0.1333333f));
            skybox.SetFloat("_Exposure", 0.03f);
            skybox.SetFloat("_SunSize", 0.276f);
            skybox.SetFloat("_SunSizeConvergence", 9.67f);
            skybox.SetFloat("_AtmosphereThickness", 0.62f);
            RenderSettings.skybox = skybox;
            RenderSettings.fogColor = new Color(0.0745098f, 0.0352941f, 0.0392157f);
        }
        foreach (GameObject candle in candles)
        {
            // Do something with each child transform
            if(candle.name == "Candle " + index)
            {   
                //Debug.Log("Child: " + candle.name + " found at index: " + index);
                GameObject bloodCandle = Instantiate(bloodCandlePrefab, candle.transform.position, candle.transform.rotation, transform);
                bloodCandle.transform.localScale = candle.transform.localScale;
                bloodCandles[index-1] = bloodCandle;
                originalCandles[index-1].SetActive(false);
                index++;
            }
            //originalCandles[index-1].SetActive(!annen);
        } 
        
    }

    void normalScene()
    {
        
        
        int index = 1;
        //Debug.Log("Normal Scene");
        foreach (GameObject bloodCandle in bloodCandles)
        {
            if(bloodCandle != null)
            {
                Destroy(bloodCandle);
            }
        }
        if(sunLight != null)
        {
            sunLight.intensity = 0.12f;
            sunLight.color = new Color(0.5568627451f, 0.5568627451f, 0.5568627451f);
            skybox.SetColor("_Tint", new Color(0.02352941f, 0.02745098f, 0.1333333f));
            skybox.SetFloat("_Exposure", 0.03f);
            skybox.SetFloat("_SunSize", 0.276f);
            skybox.SetFloat("_SunSizeConvergence", 9.67f);
            skybox.SetFloat("_AtmosphereThickness", 0.62f);
            RenderSettings.skybox = skybox;
            RenderSettings.fogColor = new Color(0.0235f, 0.0275f, 0.1333f);
        }
        
        // Iterate through each child transform
        foreach(GameObject candle in originalCandles)
        {
            // Do something with each child transform
            originalCandles[index-1].SetActive(annen);
            index++;
            
        }
        annen = !annen;
    }
}
