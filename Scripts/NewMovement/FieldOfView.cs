using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FieldOfView : MonoBehaviour, ICanSeeTarget
{
    [SerializeField] float radius;
    [SerializeField] bool showFOVDebug;
    [SerializeField] bool checkFOV;

    public void setCheckFOV(bool check)
    {
        checkFOV = check;
    }

    public bool getShowFOVDebug()
    {
        return showFOVDebug;
    }

    public float getRadius()
    {
        return radius;
    }

    [Range(0, 360)] [SerializeField] float angle;

    public float getAngle()
    {
        return angle;
    }

    [SerializeField] GameObject playerRef;

    public GameObject getPlayerRef()
    {
        return playerRef;
    }

    [SerializeField] LayerMask targetMask;

    public LayerMask getTargetMask()
    {
        return targetMask;
    }

    [SerializeField] LayerMask obstructionMask;

    public LayerMask getObstructMask()
    {
        return obstructionMask;
    }

    [SerializeField] bool canSeePlayer;

    public bool getCanSeePlayer()
    {
        return canSeePlayer;
    }

    [SerializeField] Transform leftEndpoint;

    public Transform getleftEndpoint()
    {
        return leftEndpoint;
    }

    [SerializeField] Transform rightEndpoint;

    public Transform getrightEndpoint()
    {
        return rightEndpoint;
    }

    private Transform target;
    
    public Transform getTarget()
    {
        return target;
    }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            canSeePlayer = ((ICanSeeTarget)this).CanSeeTarget();
        }
    }

    void FixedUpdate()
    {
        Vector3 leftAngle = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 rightAngle = DirectionFromAngle(transform.eulerAngles.y, angle / 2);
        leftEndpoint.position = transform.position + leftAngle * radius;
        rightEndpoint.position = transform.position + rightAngle * radius;
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    bool ICanSeeTarget.CanSeeTarget()
    {
        //Collider[] rangeChecks = new Collider[10];
        if (checkFOV)
        {
            //int overlap = Physics.OverlapSphereNonAlloc(transform.position, radius, rangeChecks, targetMask);
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (rangeChecks.Length != 0)
            {
                target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
