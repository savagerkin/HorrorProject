using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_raycast : MonoBehaviour
{
    // Start is called before the first frame update
    Ray ray; 

    RaycastHit hit;

    private bool isIce = false;

    [SerializeField] private LayerMask boundries;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera cam;
    private float targetFOV = 40f;
    private float startFov = 80f;
    private float duration = 5f;
    private Coroutine fovcouroutine;
    bool islooking = false;

    

    
    //private Transform selection;

    [Header("Boundrie materials")]

    [SerializeField] private Material mat_default;
    private float distance = 0f;
    
    private int index;
    

    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        OnIce();
        ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if(Physics.Raycast(ray, out hit, 50f, boundries))
        {
            if (hit.transform != null)
            {
                var selection = hit.transform;
                var renderer = selection.GetComponent<Renderer>();
                var material = renderer.material;
                var color = material.color;
                if(hit.distance > 0.0f)
                {
                    distance = 50 - hit.distance;
                    color.a =2 * (distance / 100);
                    material.color = color;
                }
                
            }
        }
        if(Physics.Raycast(ray, out hit, 3f, LayerMask.GetMask("Selectable")))
        {
            if(hit.transform != null && !islooking)
            {
                islooking = true;
                if(fovcouroutine != null)
                {
                    StopCoroutine(fovcouroutine);
                }
                fovcouroutine = StartCoroutine(ChangeFOV(cam.fieldOfView,targetFOV,duration));
            }
        }
        else if(islooking)
        {
            islooking = false;
            if(fovcouroutine != null)
            {
                StopCoroutine(fovcouroutine);
            }
            fovcouroutine = StartCoroutine(ChangeFOV(cam.fieldOfView,startFov,1f));
        }
        
    }

    void OnIce()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f, LayerMask.GetMask("Ground") ))
        {
            if (hit.collider.CompareTag("Ice"))
            {
                isIce = true;
            }
            else
            {
                isIce = false;
            }
        }
    }
    private IEnumerator ChangeFOV(float startFov, float endFOV, float duration)
    {
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            cam.fieldOfView = Mathf.Lerp(startFov, endFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.fieldOfView = endFOV;
    }

    public bool getIce
    {
        get => isIce; 
    }

        
}
