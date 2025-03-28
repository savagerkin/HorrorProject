using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressDoor : MonoBehaviour
{
    public float interactionDistance;
    public string doorOpenAnimName, doorCloseAnimName;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject.tag  == "Door")
            {
                GameObject doorParent = hit.collider.transform.parent.parent.gameObject;
                Animator doorAnim = doorParent.GetComponent<Animator>();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))
                    {
                        doorAnim.ResetTrigger("open");
                        doorAnim.SetTrigger("close");
                    }

                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName))
                    {
                        doorAnim.ResetTrigger("close");
                        doorAnim.SetTrigger("open");
                    }
                }
                else
                {
                }
            }
        }
    }
}