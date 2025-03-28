using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class DoorScript : MonoBehaviour
{
    private HingeJoint hingeJoint;
    private Transform doorTransform;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private AudioClip[] doorClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            handAnimator.SetBool("doorRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            handAnimator.SetBool("doorRange", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(doorTransform.localEulerAngles.y >= 0 && doorTransform.localEulerAngles.y < 45 || doorTransform.localEulerAngles.y <= 360 && doorTransform.localEulerAngles.y > 315)
        {
            hingeJoint.useSpring = true;
        }
        else
        {
            hingeJoint.useSpring = false;
        }
    }

    private AudioClip GetRandomClip()
    {
        return doorClips[UnityEngine.Random.Range(0, doorClips.Length)];
    }
    
    /*
     * AudioClip clip = GetRandomClip();
            audioSource.volume = 0.5f;
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(clip);
     */
}
