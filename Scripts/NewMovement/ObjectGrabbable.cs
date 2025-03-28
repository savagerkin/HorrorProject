using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectGrabbable : MonoBehaviour
    
{
    private Rigidbody objectRB; //The object's rigidbody
    private Transform objectGrabPointTransform; //The point where the object will be grabbed at
    [Header("For Notes")]
    [SerializeField] private bool isNote = false;
    public bool IsNote
    {
        get { return isNote; }
        set { isNote = value; }
    }

    private void Awake()
    {
        objectRB = GetComponent<Rigidbody>();
        
    }

    private Vector3 originalTransform;
    private Notebook notebook;
    public void Grab(Transform objectGrabPoint)
    {
        
        objectGrabPointTransform = objectGrabPoint; //Set the objectGrabPointTransform to the objectGrabPoint
        objectRB.useGravity = false; //Disable gravity
        objectRB.velocity = Vector3.zero; //Set the velocity to zero
        objectRB.freezeRotation = true; //Freeze rotation so it dosnet spasm out
        objectRB.drag = 5f; //Add drag to the object, so it dosent spasm out
        originalTransform =
            objectGrabPointTransform.localPosition; //Save the original position of the objectGrabPointTransform     
    }

    public void Drop()
    {
        if (objectGrabPointTransform != null)
        {
            parentObject =
                objectGrabPointTransform.parent.gameObject; //Get the parent object of the objectGrabPointTransform
            CameraTest
                cameraScript =
                    parentObject.GetComponent<CameraTest>(); //Get the CameraTest script from the parent object
            cameraScript.CameraUnlocked = true; //Set the CameraUnlocked bool to true
            objectGrabPointTransform.localPosition =
                originalTransform; //Set the objectGrabPointTransform back to its original position
            if (IsNote)
            {
                // Get the Notebook component and call the CloseNotebook method
                Notebook notebook = GetComponent<Notebook>();
                if (notebook != null)
                {
                    notebook.CloseNotebook();
                }
            }
        }

        objectGrabPointTransform = null;
        objectRB.useGravity = true;
        objectRB.freezeRotation = false;
        objectRB.drag = 0.5f;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position,
                Time.deltaTime * lerpSpeed); //Lerp the object to the objectGrabPointTransform position

            // Get mouse wheel scroll delta
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

            // Move objectGrabPointTransform forward or backward based on scrollDelta
            float scrollSpeed = 1f; // Adjust this value to your liking
            if (objectGrabPointTransform.localPosition.z > 0.5f && scrollDelta < 0)
            {
                objectGrabPointTransform.position += objectGrabPointTransform.forward * scrollDelta * scrollSpeed;
            }

            if (objectGrabPointTransform.localPosition.z < 2.0f && scrollDelta > 0)
            {
                objectGrabPointTransform.position += objectGrabPointTransform.forward * scrollDelta * scrollSpeed;
            }


            objectRB.MovePosition(newPosition);

            parentObject =
                objectGrabPointTransform.parent.gameObject; //Get the parent object of the objectGrabPointTransform
            CameraTest cameraScript = parentObject.GetComponent<CameraTest>();
            if (cameraScript != null && cameraScript.CameraUnlocked == false)
            {
                // Get mouse movement
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                
                // Apply rotation
                float rotationSpeed = 5f; // Adjust this value to your liking
                transform.Rotate(new Vector3(-mouseY, mouseX, 0) * rotationSpeed,
                    Space.Self); // Rotate the objectGrabPointTransform
                if (Input.GetKey(KeyCode.X))
                {
                    // Rotate positively around the Z axis
                    transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed, Space.Self);
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    // Rotate negatively around the Z axis
                    transform.Rotate(new Vector3(0, 0, -1) * rotationSpeed, Space.Self);
                }
            }
        }
    }

    private GameObject parentObject;

    private void Update()
    {
        if (objectGrabPointTransform != null && Input.GetKeyDown(KeyCode.Mouse1))
        {
            parentObject =
                objectGrabPointTransform.parent.gameObject; //Get the parent object of the objectGrabPointTransform
            CameraTest cameraScript = parentObject.GetComponent<CameraTest>();
            if (cameraScript != null && cameraScript.CameraUnlocked == false)
            {
                cameraScript.CameraUnlocked = true; //Set the CameraUnlocked bool to true
            }
            else if (cameraScript != null && cameraScript.CameraUnlocked == true)
            {
                cameraScript.CameraUnlocked = false; //Set the CameraUnlocked bool to false
            }
        }
    }
}