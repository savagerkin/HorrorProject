using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] lanternMoveClips;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (rb.velocity.magnitude > 0.6f && !isMoving)
        {
            isMoving = true;
            AudioClip clip = GetRandomClip();
            audioSource.volume = 0.15f;
            audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
            audioSource.PlayOneShot(clip);
        }
        else if (rb.velocity.magnitude <= 0.3f && isMoving)
        {
            isMoving = false;
        }
    }

    private AudioClip GetRandomClip()
    {
        return lanternMoveClips[UnityEngine.Random.Range(0, lanternMoveClips.Length)];
    }
}
