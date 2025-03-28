using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnowEffect : MonoBehaviour
{

    private bool _ResetTimer;
    private float _Timer;

    private float iceFactor;

    Collider playerCollider;
    private Player_raycast playerRaycast;
    [SerializeField] private Material snowFlake;

    [SerializeField] private Light _Light;
    [SerializeField] private float _Intensity = 1.0f;
    [SerializeField] private float _Power = 4f;
    [SerializeField] private Material material; // Added 'private Material' to declare the type of the material variable
    // Start is called before the first frame update
    private float _IntensityValue;
    void Start()
    {
        material.SetFloat("_Intensity", 0);
        material.SetFloat("_Power", 4);
        playerCollider = GetComponent<Collider>();

        _Timer = 0;
        playerRaycast = GetComponent<Player_raycast>();
    }

    // Update is called once per frame

    void Update()
    {
        if (playerRaycast.getIce)
        {
            iceFactor = 0.02f;
        }
        else
        {
            iceFactor = 0.0f;
        }
        Color lightColor = _Light.color;
        _IntensityValue = Mathf.Log(_Timer + 1, 5);
        snowFlake.color = lightColor * _Intensity;
        if (_ResetTimer)
        {
            _Timer = 0;
            _ResetTimer = false;
        }
        if (_Timer <= 20.0f)
        {
            Freezing();
        }
        if (_Timer >= 20.0f)
        {
            Freezing_to_death();
        }
        if (_Timer != 40.0f)
        {
            _Timer += Time.deltaTime + iceFactor;
            _Timer = Mathf.Min(_Timer, 40.0f);
        }
        if (_Timer == 40.0f)
        {
            // Debug.Log("You are dead");
            //boriko buraya ölme ekranı gelecek
        }
    }


    void Freezing()
    {
        material.SetFloat("_Intensity", _IntensityValue);
    }

    void Freezing_to_death()
    {
        float elapsedTime = _Timer - 20.0f;
        float powerValue = Mathf.Lerp(4.0f, 0.5f, elapsedTime / 15.0f);
        material.SetFloat("_Power", powerValue);
    }


    private void OnTriggerStay(Collider other)
    {
        // Ensure this trigger is only for the player's own collider

        if (other.CompareTag("Light") || other.CompareTag("Inside"))
        {
            // Debug.Log(other.gameObject.name);
            if (_Timer > 0.0f)
            {
                _Timer -= Time.deltaTime * 2;
                _Timer = Mathf.Max(_Timer, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ensure this trigger is only for the player's own collider
        if (playerCollider.tag != "RenderDistance" &&
            (other.gameObject.CompareTag("Light") || other.gameObject.CompareTag("Inside") && gameObject.CompareTag("Player")))
        {
            _ResetTimer = true;
        }
    }

}
