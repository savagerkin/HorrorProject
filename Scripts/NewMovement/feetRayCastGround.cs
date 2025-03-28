using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class feetRayCastGround : MonoBehaviour
{
    [SerializeField] private float raycastRange = 10.0f; // Set this to the range you want
    [SerializeField] private float XOffset = 0.0f;
    [SerializeField] private float YOffset = 0.25f;
    [SerializeField] private float ZOffset = 0.0f;

    private int previousState = 0;
    [SerializeField] private bool left = true; //left feet true, right feet false

    [SerializeField] private bool globalDown; //test variable for seeing if the global down is better or local

    private Vector3 transformTest;
    private bool footOfGround = false;
    private IKFeetPlacement ikFeetPlacement;

    private void Start()
    {
        ikFeetPlacement = transform.parent.parent.parent.parent.parent.GetComponent<IKFeetPlacement>();
        Debug.Log("ikFeetPlacement");
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (globalDown)
        {
            transformTest = UnityEngine.Vector3.down;
        }
        else
        {
            transformTest = -transform.forward;
        }

        if (Physics.Raycast(transform.position + new Vector3(XOffset, YOffset, ZOffset), transformTest,
                out hit, raycastRange))
        {
            Debug.DrawRay(transform.position + new Vector3(XOffset, YOffset, ZOffset),
                transformTest * raycastRange, Color.red);
            snowFootMark snowFootMarkScript = hit.transform.GetComponent<snowFootMark>();
            if (snowFootMarkScript)
            {
                if (ikFeetPlacement.feetSide == 0 && previousState == 1)
                {
                    if (!left)
                    {
                        snowFootMarkScript.LeftStep(hit.point);
                    }

                    previousState = 0;
                }
                if (ikFeetPlacement.feetSide == 1 && previousState == 0)
                {
                    if (left)
                    {
                        snowFootMarkScript.RightStep(hit.point);
                    }

                    previousState = 1;
                }
            }
        }
    }
}