using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreakCheck : MonoBehaviour
{
    public bool onCreak = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Creak"))
        {
            onCreak = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Creak"))
        {
            onCreak = false;
        }
    }
}
