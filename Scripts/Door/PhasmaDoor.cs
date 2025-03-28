using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasmaDoor : MonoBehaviour
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform distCheck;

    [SerializeField] private Transform hinge;

    [SerializeField] private float moveSpeed;
    [SerializeField] Vector2 roationConstraints;

    private bool movingDoor;
    private float rotation;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = distCheck.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hit, 3))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    movingDoor = true;
                }
            }
        }

        if (movingDoor)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                movingDoor = false;
            }

            targetPosition = playerCam.position + playerCam.forward * 2;
        }

        rotation += Mathf.Clamp(-GetRotation() * 5000 * Time.deltaTime, -moveSpeed, moveSpeed);
        rotation = Mathf.Clamp(rotation, roationConstraints.x, roationConstraints.y);
        hinge.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    float GetRotation()
    {
        float firstDistance = (distCheck.position - targetPosition).sqrMagnitude;
        hinge.Rotate(Vector3.up);
        float secondDistance = (distCheck.position - targetPosition).sqrMagnitude;
        hinge.Rotate(-Vector3.up);
        return secondDistance - firstDistance;
    }
}