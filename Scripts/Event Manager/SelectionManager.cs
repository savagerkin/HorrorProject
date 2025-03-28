using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;

    [SerializeField] private LayerMask selectable;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private AudioClip lightSwitchSound;
    [SerializeField] private AudioSource audioSource;
    
    private bool flag = false; 
    
    private KeyCode selectKey = KeyCode.E;
    private Transform _selection;

    [SerializeField] private float selectablePoint = 5f;
    void Update()
    {
        if(_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if(Physics.Raycast(ray, out hit, selectablePoint, selectable))
        {
            var selection = hit.transform;

            if (selection != null)
            {
                var selectionRenderer = selection.GetComponent<Renderer>();

                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                    
                    if(Input.GetKeyDown(selectKey))
                    {
                        if (selectionRenderer.gameObject.CompareTag("Light"))
                        {
                            audioSource.volume = 0.1f;
                            audioSource.PlayOneShot(lightSwitchSound);
                        }
                        var selectedCandle = selection.GetChild(0).gameObject;
                        selectedCandle.SetActive(flag);
                        flag = !flag;
                    }
                }
            }
            _selection = selection;
        }
    } 
}
