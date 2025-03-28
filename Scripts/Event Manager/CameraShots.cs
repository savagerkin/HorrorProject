using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShots : MonoBehaviour
{

    [SerializeField] private GameObject shot1,shot2,shot3;
    [SerializeField] private GameObject normal_cam;
    [SerializeField] private Animator anim;
    private bool isAnimating = false;

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            if(gameObject.name == "sideShotZone")
            {
                normal_cam.SetActive(false);
                shot2.SetActive(true);
            }
            if(gameObject.name == "CameraSec_1")
            {
                normal_cam.SetActive(false);
                shot1.SetActive(true);
            }
            
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
            
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Exit");
            shot2.SetActive(false);
            shot1.SetActive(false);
            normal_cam.SetActive(true);
        }
    }

    private IEnumerator SwitchBackToNormalCam(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        shot1.SetActive(false);
        shot2.SetActive(false);
        normal_cam.SetActive(true);
        isAnimating = false;
    }
    private void Update()
    {

    }

}
