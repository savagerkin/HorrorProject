using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreParentCollider : MonoBehaviour
{
    [SerializeField] Collider parentcollider;
    [SerializeField] Collider childCollider;

    // Start is called before the first frame update
    void Start()
    {
        parentcollider = GetComponent<Collider>();

        Physics.IgnoreCollision(parentcollider, childCollider);
    }
}
