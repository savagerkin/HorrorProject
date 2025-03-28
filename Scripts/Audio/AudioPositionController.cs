using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPositionController : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    private Vector3[] positions = new Vector3[8];
    private AudioSource audioSource;
    private float octagonRadius = 3.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Vector3 initialPosition = transform.position;

        if (sounds.Length == 0)
        {
            Debug.LogWarning("No sounds provided. Attach at least one sound to the AudioPositionController script.");
            return;
        }

        // Calculate the positions of an octagon around the initial position
        for (int i = 0; i < 8; i++)
        {
            float angleDegrees = 45 * i;
            float angleRadians = Mathf.PI / 180 * angleDegrees;
            positions[i] = new Vector3(initialPosition.x + octagonRadius * Mathf.Cos(angleRadians), 0, initialPosition.z + octagonRadius * Mathf.Sin(angleRadians));
        }

        StartCoroutine(ChangePositionAndPlaySound());
    }

    IEnumerator ChangePositionAndPlaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            int randomPositionIndex = Random.Range(0, positions.Length);
            int randomSoundIndex = Random.Range(0, sounds.Length);
            transform.position = positions[randomPositionIndex];
            audioSource.clip = sounds[randomSoundIndex];
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }
}
