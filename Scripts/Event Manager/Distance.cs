using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{

    [SerializeField] Transform player;
    float distance = 0f; 
    GameObject cube;

    [SerializeField] GameObject canvas;  

    // Start is called before the first frame update
    void Start()
    {
        cube = gameObject;
        cube.GetComponent<MeshRenderer>().enabled = false;
        canvas.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            distance = Vector3.Distance(player.position, transform.position);
            Debug.Log("Distance to player: " + distance);
        }
        if(distance < 3.0f)
        {
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            if (canvas.transform.childCount > 1)
            {
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (canvas.transform.childCount > 2)
            {
                canvas.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            if (canvas.transform.childCount > 1)
            {
                canvas.transform.GetChild(1).gameObject.SetActive(false);
            }
            if (canvas.transform.childCount > 2)
            {
                canvas.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
}
