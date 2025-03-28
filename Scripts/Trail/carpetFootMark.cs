using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class carpetFootMark : MonoBehaviour
{
    [SerializeField] private GameObject footMarkPrefab;
    // Start is called before the first frame update
    private Vector3 lastPositionVector3;
    
    [SerializeField] private AudioClip[] carpetClips;
    
    [SerializeField] AudioSource audioSource;
    
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    void TimeCheck()
    {
    }

    private int lastMinute = 0;
    [SerializeField] private GameObject orientation;
    private bool footLeft = false;
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;

    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
             Input.GetKey(KeyCode.S)) && TimeManager.totalMinutesPassed % 3 == 0 &&
            TimeManager.totalMinutesPassed != lastMinute && footLeft)
        {
            lastMinute = TimeManager.totalMinutesPassed;
            lastPositionVector3 = other.contacts[0].point;
            AudioClip clip = GetRandomClip();
            audioSource.volume = 0.25f;
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);
            
            // Get the current rotation and subtract 90 from the x component
            float currentRotationY = orientation.transform.rotation.eulerAngles.y;
            // Instantiate the footMarkPrefab with the new rotation
            GameObject footMarkInstance = Instantiate(footMarkPrefab, leftFoot.transform.position,
                Quaternion.Euler(90,currentRotationY,0));

            // Destroy the footMarkInstance after 5 seconds
            Destroy(footMarkInstance, 5);
            footLeft = false;
        }
        else if (other.gameObject.CompareTag("Player") &&
                 (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
                  Input.GetKey(KeyCode.S)) && TimeManager.totalMinutesPassed % 3 == 0 &&
                 TimeManager.totalMinutesPassed != lastMinute && !footLeft)
        {
            lastMinute = TimeManager.totalMinutesPassed;
            lastPositionVector3 = other.contacts[0].point;
            
            AudioClip clip = GetRandomClip();
            audioSource.volume = 0.25f;
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);

            // Get the current rotation and subtract 90 from the x component
            float currentRotationY = orientation.transform.rotation.eulerAngles.y;
            // Instantiate the footMarkPrefab with the new rotation
            GameObject footMarkInstance = Instantiate(footMarkPrefab, rightFoot.transform.position,
                Quaternion.Euler(-90,currentRotationY,180));
            
            // Destroy the footMarkInstance after 5 seconds
            Destroy(footMarkInstance, 5);
            footLeft = true;
        }
    }
    
    private AudioClip GetRandomClip()
    {
        return carpetClips[UnityEngine.Random.Range(0, carpetClips.Length)];
    }
    
}