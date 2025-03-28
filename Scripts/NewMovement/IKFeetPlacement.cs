using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKFeetPlacement : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool debugOn;
    public LayerMask layerMask; // Select all layers that foot placement applies to.

    [Range(0, 1f)] public float
        DistanceToGround; // Distance from where the foot transform is to the lowest possible position of the foot.

    public int feetSide;

    public void FootStepEvent(int feetSide) // sag 0, sol 1
    {
        this.feetSide = feetSide;
    }

    private Transform playerMeshTransform;

     private void Update()
     {
         var transform1 = playerMeshTransform.transform;
         var rotation = transform1.localRotation;
         rotation.y = 0;
         transform1.localRotation = rotation;
     }
    private void Start()
    {
        playerMeshTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("IKLeftFootWeight"));
            //left foot
            RaycastHit hit;
            Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (debugOn) Debug.DrawRay(ray.origin, ray.direction * (DistanceToGround + 1), Color.red, 1f);
                Vector3 footPosition = hit.point;
                footPosition.y += DistanceToGround;
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }

            // Right foot
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("IKRightFootWeight"));
            ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (debugOn) Debug.DrawRay(ray.origin, ray.direction * (DistanceToGround + 1), Color.blue, 1f);

                Vector3 footPosition = hit.point;
                footPosition.y += DistanceToGround;
                anim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
    }
}