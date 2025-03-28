using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class snowFootMark : MonoBehaviour
{
    [SerializeField] private GameObject footMarkPrefab;

    // Start is called before the first frame update
    private Vector3 lastPositionVector3;

    [FormerlySerializedAs("carpetClips")] [SerializeField]
    private AudioClip[] snowClips;

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

    private int lastMinuteL = 0;
    private int lastMinuteR = 0;
    [SerializeField] private GameObject orientation;
    [SerializeField] private float soundVolume = 0.3f;
    private bool footLeft = false;
    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;

    public void LeftStep(Vector3 hitPosition)
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            lastMinuteL = TimeManager.totalMinutesPassed;
            AudioClip clip = GetRandomClip();
            audioSource.volume = soundVolume;
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);

            // Get the current rotation and subtract 90 from the x component
            float currentRotationY = orientation.transform.rotation.eulerAngles.y;
            if (footMarkPrefab != null)
            {
                // Instantiate the footMarkPrefab with the new rotation
                GameObject footMarkInstance = Instantiate(footMarkPrefab, hitPosition + new Vector3(0, -1, 0),
                    Quaternion.Euler(270, currentRotationY + 90, 270));

                // Destroy the footMarkInstance after 5 seconds

                Destroy(footMarkInstance, 15);
            }
        }
    }

    public void RightStep(Vector3 hitPosition)
    {
        if (TimeManager.totalMinutesPassed - 1 > lastMinuteR)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S))
            {
                lastMinuteR = TimeManager.totalMinutesPassed;
                AudioClip clip = GetRandomClip();
                audioSource.volume = soundVolume;
                audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
                audioSource.PlayOneShot(clip);

                // Get the current rotation and subtract 90 from the x component
                float currentRotationY = orientation.transform.rotation.eulerAngles.y;
                // Instantiate the footMarkPrefab with the new rotation
                if (footMarkPrefab != null)
                {
                    GameObject footMarkInstance = Instantiate(footMarkPrefab, hitPosition + new Vector3(0, -1, 0),
                        Quaternion.Euler(270, currentRotationY + 90, 270));

                    // Destroy the footMarkInstance after 5 seconds
                    Destroy(footMarkInstance, 15);
                }
            }
        }
    }

    /*
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("LFeet"))
        {
            Debug.Log("Left Foot");
        }

        if (other.gameObject.CompareTag("LFeet") &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
             Input.GetKey(KeyCode.S)) && TimeManager.totalMinutesPassed - lastMinute > 1)
        {
            lastMinute = TimeManager.totalMinutesPassed;
            lastPositionVector3 = other.contacts[0].point;

            AudioClip clip = GetRandomClip();
            audioSource.volume = 0.3f;
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);

            // Get the current rotation and subtract 90 from the x component
            float currentRotationY = orientation.transform.rotation.eulerAngles.y;
            // Instantiate the footMarkPrefab with the new rotation
            GameObject footMarkInstance = Instantiate(footMarkPrefab, leftFoot.transform.position,
                Quaternion.Euler(90, currentRotationY, 0));

            // Destroy the footMarkInstance after 5 seconds
            Destroy(footMarkInstance, 5);
            footLeft = false;
        }
        else if (other.gameObject.CompareTag("RFeet") &&
                 (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) ||
                  Input.GetKey(KeyCode.S)) && TimeManager.totalMinutesPassed - lastMinute > 1)
        {
            lastMinute = TimeManager.totalMinutesPassed;
            lastPositionVector3 = other.contacts[0].point;

            AudioClip clip = GetRandomClip();
            audioSource.volume = 0.3f;
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);

            // Get the current rotation and subtract 90 from the x component
            float currentRotationY = orientation.transform.rotation.eulerAngles.y;
            // Instantiate the footMarkPrefab with the new rotation
            GameObject footMarkInstance = Instantiate(footMarkPrefab, rightFoot.transform.position,
                Quaternion.Euler(-90, currentRotationY, 180));

            // Destroy the footMarkInstance after 5 seconds
            Destroy(footMarkInstance, 5);
            footLeft = true;
        }
    }
    */
    private AudioClip GetRandomClip()
    {
        return snowClips[UnityEngine.Random.Range(0, snowClips.Length)];
    }
}