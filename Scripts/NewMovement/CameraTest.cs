using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    private bool _cameraUnlocked = true;

    public bool CameraUnlocked
    {
        get { return _cameraUnlocked; }
        set { _cameraUnlocked = value; }
    }

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;
    [SerializeField] Transform playerBody = null; // Reference to the player's transform

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        sensX = 100 * PlayerPrefs.GetFloat("sensitivity", 1f);
        sensY = 100 * PlayerPrefs.GetFloat("sensitivity", 1f);

        if (_cameraUnlocked)
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            // Update the player's rotation to match the camera's rotation
            playerBody.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void SetSensX(float newSensX)
    {
        sensX = newSensX;
    }

    public void SetSensY(float newSensY)
    {
        sensY = newSensY;
    }
}