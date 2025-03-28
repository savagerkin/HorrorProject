using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearCabin : MonoBehaviour
{
    [SerializeField] GameObject burningTree;
    [SerializeField] GameObject worldterrain;
    ParticleSystem fire;
    bool isBurning = false;
    //one time event only so bool is enough
    bool change = true;
    bool treeFall = true;
    bool treechangeflag = false;
    float _mushroomTimer = 0;
    float _treeTimer = 0;
    TreeChange treeChange;
    GameObject []falling_trees;
    GameObject []falling_trees_right;

    [SerializeField] private GameObject fallingTreeParent;
    
    [Header("Effects")]
    [SerializeField] private GameObject treeFallEffect;
    [SerializeField] private GameObject mushroomTreeEffect;
    [SerializeField] private GameObject NearHouse;
    
    [SerializeField] private Transform BurningParticleSystem;
    [SerializeField] private float mushrromTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {   
        if(worldterrain != null)
        {
            treeChange = worldterrain.GetComponent<TreeChange>();
        }
        foreach (Transform child in burningTree.transform)
        {
            if (child == BurningParticleSystem)
            {
                fire = child.GetComponent<ParticleSystem>();
            }
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
        if(change == false)
        {
            _mushroomTimer += Time.deltaTime;
            if(_mushroomTimer > mushrromTimer && (treechangeflag == true))
            {
                treeChange.mushroomToNormal();
                treechangeflag = false;    
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string name = gameObject.name;
        if(other.gameObject.CompareTag("Player"))
        {
            if(name == NearHouse.name)
            {
                fire.Stop();
            }
            if(name == mushroomTreeEffect.name)
            {
                //change bool is used to prevent multiple calls to treechangetest
                if(change)
                {
                    treeChange.treeChangeToMushroom();
                    change = false;
                    treechangeflag = true;
                }
            }
            if(name == treeFallEffect.name)
            {
                if(treeFall)
                {   
                    treeFall = false;
                    fallingTreeEvent();
                }
                
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        string name = gameObject.name;
        if(other.gameObject.tag == "Player")
        {
            if(name == NearHouse.name)
            {
                fire.Play();
            }
        }
    }
    void fallingTreeEvent()
    {
        int leftIndex = 0;      
        falling_trees = new GameObject[fallingTreeParent.transform.childCount];
        foreach(Transform child in fallingTreeParent.transform)
        {   
            falling_trees[leftIndex] = child.gameObject;
            leftIndex++;
        }
        float fall_time = 1f; 
        StartCoroutine(MakeTreesFallWithDelay(falling_trees, fall_time));
        
    }

    // IEnumerator RotateTreeWithDelay(GameObject[] trees, float delay)
    // {
    //     foreach(GameObject tree in trees)
    //     {
    //         delay = Random.Range(0.2f, 1f);
    //         if(tree != null)
    //         {
    //             StartCoroutine(RotateTree(tree));
    //             yield return new WaitForSeconds(delay);
    //         }
    //     }
    // }
    IEnumerator MakeTreesFallWithDelay(GameObject[] trees, float delay)
    {
        float force = -1500f;
        for(int i = falling_trees.Length - 1 ; i >= 0; i--)
        {
            if (falling_trees[i] != null)
            {
                Rigidbody rb = falling_trees[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true; // Ensure gravity is applied
                    force*= -1;
                    Vector3 torque = new Vector3(force,0,0);
                    rb.AddTorque(torque, ForceMode.Impulse); // Apply a torque
                }
                yield return new WaitForSeconds(delay); // Wait for the specified delay
            }
        }
    }
    // IEnumerator RotateTree(GameObject tree)
    // {
    //     Quaternion initialRotation = tree.transform.rotation;
    //     Quaternion targetRotation1 = initialRotation * Quaternion.Euler(-25, 0, 0);
    //     Quaternion targetRotation2 = targetRotation1 * Quaternion.Euler(-50, 0, 0);
    //
    //     float elapsedTime = 0f;
    //     float duration1 = 1.5f;
    //     float duration2 = 0.7f;
    //
    //     // Rotate to -25 degrees in 2 seconds
    //     while (elapsedTime < duration1)
    //     {
    //         tree.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation1, elapsedTime / duration1);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     tree.transform.rotation = targetRotation1;
    //
    //     elapsedTime = 0f;
    //
    //     // Rotate to -65 degrees in 1 second
    //     while (elapsedTime < duration2)
    //     {
    //         tree.transform.rotation = Quaternion.Slerp(targetRotation1, targetRotation2, elapsedTime / duration2);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     tree.transform.rotation = targetRotation2;
    // }

  
}
