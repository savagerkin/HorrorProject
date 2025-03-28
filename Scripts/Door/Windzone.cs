using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Windzone : MonoBehaviour
{
    [SerializeField] private float windForce = 0f;
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private GameObject playerMesh;
    bool flag = false;
    private void Start()
    {
        StartCoroutine(RotateWind());
        
    }
    private void OnTriggerStay(Collider other)
    {
        var hitObj = other.gameObject;
        if (hitObj.CompareTag("Door"))
        {
            var rb = hitObj.GetComponent<Rigidbody>();
            var dir = transform.forward;
            rb.AddForce(dir * windForce);
        }
    }

    private IEnumerator RotateWind()
    {
        while (true)
        {
            windForce = -1 * windForce;
            //flag = !flag;
            yield return new WaitForSeconds(rotateSpeed);
        }
    }

    private void setActivePlayerMesh()
    {

    }    

}
