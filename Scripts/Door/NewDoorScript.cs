using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorScript : MonoBehaviour
{
    private HingeJoint hingeJoint;
    private Transform doorTransform;
    private AudioSource audioSource;

    private bool creaked = false;

    [SerializeField] AudioClip doorOpeningSound;

    [SerializeField] AudioClip doorClosingSound;

    [SerializeField] AudioClip doorCreakingSound;
    
    private bool wasOpen;

    bool canPlayStopSound = false;

    Quaternion previousRotation;

    private void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();
        if (hingeJoint == null)
        {
            Debug.Log("No HingeJoint component attached to the object.");
        }
        
        doorTransform = GetComponent<Transform>();
        if (doorTransform == null)
        {
            Debug.Log("No Transform component attached to the object.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("No AudioSource component attached to the object.");
        }
        if (doorOpeningSound == null)
        {
            Debug.Log("No doorOpeningSound AudioClip attached to the object.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody doorRigidbody = doorTransform.GetComponent<Rigidbody>();

        if ((doorTransform.localEulerAngles.y >= 0 && doorTransform.localEulerAngles.y < 45) ||
            (doorTransform.localEulerAngles.y <= 360 && doorTransform.localEulerAngles.y > 315))
        {
            hingeJoint.useSpring = true;
        }
        else
        {
            hingeJoint.useSpring = false;
        }

        // Checks if the door is opening/closing and plays the creaking sound
        if (!creaked && 
            ((doorTransform.localEulerAngles.y >= 40 && doorTransform.localEulerAngles.y < 50) || 
             (doorTransform.localEulerAngles.y <= 320 && doorTransform.localEulerAngles.y > 310)))
        {
            audioSource.volume = 0.3f;
            canPlayStopSound = true;
            creaked = true;
            audioSource.PlayOneShot(doorCreakingSound); 
        }

        // Checks if the door is closed and plays the closing sound
        if (canPlayStopSound && (doorRigidbody.velocity.magnitude <= 0.1) &&
             ((doorTransform.localEulerAngles.y > 359 && doorTransform.localEulerAngles.y <= 360) ||
              (doorTransform.localEulerAngles.y < 1 && doorTransform.localEulerAngles.y >= 0)))
        {
            audioSource.volume = 0.15f;
            canPlayStopSound = false;
            creaked = false;
            audioSource.PlayOneShot(doorClosingSound);
        }
    }
    
    // Checks if the player has collided with the door and plays the opening sound
    void OnCollisionEnter(Collision collision)
    {
        if (doorOpeningSound != null && collision.gameObject.tag == "Player" && 
            ((doorTransform.localEulerAngles.y > 350 && doorTransform.localEulerAngles.y <= 360) || 
             (doorTransform.localEulerAngles.y < 10 && doorTransform.localEulerAngles.y >= 0)))
        {
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(doorOpeningSound);
            creaked = false;
        }
        else if (collision.gameObject.tag == "Player" && 
                 ((doorTransform.localEulerAngles.y > 70 && doorTransform.localEulerAngles.y <= 90) || 
                  (doorTransform.localEulerAngles.y < 290 && doorTransform.localEulerAngles.y >= 270)))
        {
            creaked = false;
        }
    }
}