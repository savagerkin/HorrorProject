
public class FenceCollider : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider colliders;
    Transform[] planks;
    void Start()
    {
        if(GetComponentInChildren<BoxCollider>().name == "Cube")
        {
            colliders = GetComponent<BoxCollider>();

        }
        planks = GetComponentsInChildren<Transform>();
        
        foreach(Transform plank in planks)
        {
            if(plank.name == "Cube.001" || plank.name == "Cube.004" || plank.name == "Cube.005"
            || plank.name == "Cube.005")
            {
                //for max y transform and x rotation edit theese two
                float randomXrotation = Random.Range(-2f,6f);
                float randomYposition = Random.Range(-0.2f,0.2f);

                plank.transform.rotation = Quaternion.Euler(plank.transform.rotation.eulerAngles.x + randomXrotation,
                plank.transform.rotation.eulerAngles.y, plank.transform.rotation.eulerAngles.z);
                
                plank.transform.position = new Vector3(plank.transform.position.x, plank.transform.position.y + randomYposition,
                plank.transform.position.z);
            }
        }
        
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class FenceCollider : MonoBehaviour
// {
//     // Start is called before the first frame update
//     BoxCollider colliders;
//     Transform[] planks;
//     void Start()
//     {
//         if(GetComponentInChildren<BoxCollider>().name == "Cube")
//         {
//             Debug.Log("Cube found");
//             colliders = GetComponent<BoxCollider>();
//
//         }
//         planks = GetComponentsInChildren<Transform>();
//         
//         foreach(Transform plank in planks)
//         {
//             if(plank.name == "Cube.001" || plank.name == "Cube.004" || plank.name == "Cube.005"
//             || plank.name == "Cube.005")
//             {
//                 //for max y transform and x rotation edit theese two
//                 float randomXrotation = Random.Range(-2f,6f);
//                 float randomYposition = Random.Range(-0.2f,0.2f);
//
//                 plank.transform.rotation = Quaternion.Euler(plank.transform.rotation.eulerAngles.x + randomXrotation,
//                 plank.transform.rotation.eulerAngles.y, plank.transform.rotation.eulerAngles.z);
//                 
//                 plank.transform.position = new Vector3(plank.transform.position.x, plank.transform.position.y + randomYposition,
//                 plank.transform.position.z);
//             }
//         }
//
//         
//             
//             if(GetComponent<Collider>().gameObject.name == "Cube")
//             {
//                 GetComponent<Collider>().enabled = false;
//             }
//             
//     }
//
//     void OnTriggerEnter(Collider other)
//     {
//         if(other.gameObject.tag == "Player")
//         {
//             if(GetComponent<Collider>().gameObject.name == "Cube")
//             {
//                 GetComponent<Collider>().enabled = true;
//             }
//             
//         }
//     }
//     void OnTriggerExit(Collider other)
//     {
//         if(other.gameObject.tag == "Player")
//         {
//             if(GetComponent<Collider>().gameObject.name == "Cube")
//             {
//                 GetComponent<Collider>().enabled = false;
//             }
//             
//         }
//     }
// }
