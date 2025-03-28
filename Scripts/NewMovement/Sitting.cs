using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sitting : MonoBehaviour
{
    [SerializeField] private GameObject player, intText, standText;
    private bool interactable, sitting;

    [SerializeField] private MeshCollider sofa1, sofa2, sofa3;
    [SerializeField] private Transform sit_pos, sit_pos2, final_pos;
    [SerializeField] private Transform player_pos;
    [SerializeField] private Transform camera;
    [SerializeField] private Vector3 old_pos;
    [SerializeField] int rotation = 1;
    private bool flag = true;


    void OnTriggerStay(Collider other)
    {
        BoxCollider boxCollider = other as BoxCollider;
        if (other.CompareTag("Sofa") && !sitting)
        {
            if(other.gameObject.name == "Cube.002")
            {
                Debug.Log("Sofa 1");
                final_pos = sit_pos;
            }
            if(other.gameObject.name == "Cube.003")
            {
                Debug.Log("Sofa 2");
                final_pos = sit_pos2;
            }
            intText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sofa"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E) && !sitting)
        {
            intText.SetActive(false);
            interactable = false;
            StartCoroutine(MoveToSitPosition(player_pos.position, final_pos.position, 1f));
            sitting = true;
        }
        else if (sitting && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(MoveToSitPosition(player_pos.position, old_pos, 1f));
            sitting = false;
        }
    }

    IEnumerator MoveToSitPosition(Vector3 _startPos, Vector3 _endPos, float _duration)
    {
        sofa1.enabled = false;
        sofa2.enabled = false;
        sofa3.enabled = false;

        old_pos = player_pos.position;

        Vector3 startPos = _startPos;
        Vector3 endPos = _endPos;
        float duration = _duration; // Duration of the transition
        float elapsed = 0.0f;
        //Quaternion startRot = camera.rotation;
        //Quaternion endRot = Quaternion.Euler(0, 180 * rotation, 0);

        while (elapsed < duration)
        {
            player_pos.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            //camera.rotation = Quaternion.Lerp(startRot,endRot,elapsed / duration);
            //Debug.Log("Camera Rotation: " + camera.rotation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        //rotation *= -1;
        //camera.rotation = Quaternion.Euler(0, 180, 0);
        player_pos.position = endPos;
        player.GetComponent<MovementTest>().enabled = false;    
        if(!sitting)
        {
            player.GetComponent<MovementTest>().enabled = true;
            sofa1.enabled = true;
            sofa2.enabled = true;
            sofa3.enabled = true;
        }
    }

    
}
