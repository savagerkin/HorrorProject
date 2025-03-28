using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomRay : MonoBehaviour
{
    [SerializeField] private float sideWaysXRotation;
    private Ray ray;
    private RaycastHit hit;
    private LayerMask mask;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = UnityEngine.Random.Range(0.2f, 0.3f);
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.clip = GetRandomAudioClip();
        audioSource.PlayOneShot(audioSource.clip);
        mask = LayerMask.GetMask("Ground"); //Setting the mask for it to check
        ray = new Ray(gameObject.transform.position, Vector3.down );
        //From the mushroom to the ground ray cast
        //If the ray hits and object has the layer "Ground", and the distance is less than 25
        if (Physics.Raycast(ray, out hit, 25f, mask))
        {
            gameObject.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            // Align the object with the ground

            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            // Create a rotation that aligns the up direction of the object with the normal of the ground

            // Generate a random angle between 0 and 360
            float randomAngle = UnityEngine.Random.Range(0, 360);

            // Create a rotation from this angle around the Y-axis
            Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);

            // Create a rotation of -90 degrees around the X-axis
            Quaternion adjustRotation = Quaternion.Euler(sideWaysXRotation, 0, 0);

            // Combine the rotations
            Quaternion finalRotation = groundRotation * randomRotation * adjustRotation;

            gameObject.transform.rotation = finalRotation;
        }
    }



    private AudioClip GetRandomAudioClip()
    {
        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);
        return audioClip[randomIndex];
    }
}