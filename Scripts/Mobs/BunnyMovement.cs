using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunN : MonoBehaviour
{
    Rigidbody rb;
    MeshRenderer bunnyMesh; 
    float gravity = -9.81f;
    [SerializeField] float jumpHeight;
    [SerializeField] int BushIndex;
    [SerializeField] GameObject[] bushes;
    float time;
    float currentTime;
    bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bunnyMesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;
        if(time >= 2f )
        {
            Jump(BushIndex, jumpHeight);
            time = 0; 
        }
    }

    void Jump(int jumpIndex, float jumpH)
    {
        Vector3 targetPosition = bushes[jumpIndex].transform.position;
        Vector3 direction = (targetPosition - rb.position);
        // Ignore vertical for direction

        // Calculate horizontal velocity (vxz) based on distance and time
        Vector3 horizontalDistance = new Vector3(direction.x, 0, direction.z);
        float vxz = horizontalDistance.magnitude / jumpH;

        // Calculate vertical velocity (vy) to reach the desired jump height
        float vy = Mathf.Sqrt(-2 * gravity * jumpHeight);

        // Set the velocity to move towards the target
        Vector3 velocity = new Vector3(direction.normalized.x * vxz, vy, direction.normalized.z * vxz);
        rb.velocity = velocity;

        // Rotate the bunny towards the target bush
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, 1f);
        isJumping = true;
    }

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Environmental")
        {
            Debug.Log("Hit");
            if(isJumping)
            {
                isJumping = false;
                rb.position = bushes[BushIndex].transform.position;
                rb.velocity = Vector3.zero;
            }
            BushIndex++;
            if(BushIndex >= bushes.Length)
            {
                BushIndex = 0;
            }
            
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Environmental")
        {
            bunnyMesh.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Environmental")
        {
            bunnyMesh.enabled = true;
        }    
    }
}
