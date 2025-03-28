using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{

    [SerializeField] private LayerMask pickUpLayer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform objectGrabPoint;

    private ObjectGrabbable grabbedObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (grabbedObject == null)
            {
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 2f,
                        pickUpLayer))
                {
                    if (hit.transform.TryGetComponent(out grabbedObject))
                    {
                        grabbedObject.Grab(objectGrabPoint);
                        Debug.Log(hit.transform.name);
                    }
                }
            }
            else
            {
                grabbedObject.Drop();
                grabbedObject = null;
            }
        }

        // Check if the "E" key is pressed and if an object is grabbed
        if (Input.GetKeyDown(KeyCode.E) && grabbedObject != null)
        {
            // Check if the grabbed object is a notebook
            if (grabbedObject.IsNote)
            {
                // Get the Notebook component and call the OpenNotebook method
                Notebook notebook = grabbedObject.GetComponent<Notebook>();
                if (notebook != null)
                {
                    notebook.SwitchOpenCloseNoteBook();
                }
            }
        }
    }
}