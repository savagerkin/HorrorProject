using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    private FieldOfView fieldOfView;
    private TreeChange treeChange;
    string birdState; 
    Transform player;
    Rigidbody playerRb;
    [SerializeField] float radius;
    [SerializeField] float minHeight, maxHeight;
    [SerializeField] string state;
    [SerializeField] Vector3 birdHeight;
    [SerializeField] GameObject playerComponents;
    [SerializeField] GameObject world;
    float time;
    bool isLanded = false;
    float landingHeight;
    bool isLandingOff = false;
    float yOffset = 5f;
    float elapsedTime;
    float currentHeight;
    float randomHeight;
    float xOffset;
    float zOffset;
    
    Vector3 positionBeforeLanding;
    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = playerComponents.GetComponentInChildren(typeof(FieldOfView)) as FieldOfView; 
        treeChange = world.GetComponent<TreeChange>();
        player = PlayerManager.instance.player.transform;   
        playerRb = player.GetComponent<Rigidbody>();
        randomHeight = Random.Range(minHeight, maxHeight);
        currentHeight = transform.position.y;
        landingHeight = Random.Range(5f,15f);
        positionBeforeLanding = player.position + new Vector3(0,minHeight,0);
        birdState = "fly";
    }


    // Update is called once per frame
    void Update()
    { 
        xOffset = Mathf.Cos(Time.time) * radius; // Circular movement on the X-axis
        zOffset = Mathf.Sin(Time.time) * radius; // Circular movement on the Z-axis
        var playerSpeed = playerRb.velocity.magnitude;
        birdHeight = transform.position;
        state = birdState;
        switch(birdState)
        {
            case "fly":
                CircularFlying();
                if(playerSpeed < 2f)
                {
                    birdState = "land";
                }
                if(playerSpeed > 2f)
                {
                    birdState = "fly";
                }
                break;
            case "land":
                LandingOnTree(landingHeight);
                if(playerSpeed < 2f)
                {
                    birdState = "land";
                }
                if(playerSpeed > 2f && isLanded)
                {
                    birdState = "takeOff";
                }
                break;
            case "takeOff":
                LandingOffTree();
                if(playerSpeed > 2f && isLandingOff)
                {
                    birdState = "fly";                    
                }
                break;
        }
        
    }

    void CircularFlying()
    {
        //isLanded = false;
        // Calculate the new position in a circular path around the player

        elapsedTime += Time.deltaTime;
        if(elapsedTime < 1f) // changing height period
        {
            float t = elapsedTime / 3f;
            yOffset = Mathf.Lerp(currentHeight, randomHeight, t);
            //transform.position = player.position + new Vector3(0, yOffset, 0);
        }
        else
        {
            randomHeight = Random.Range(minHeight, maxHeight);
            currentHeight = yOffset;
            elapsedTime = 0;
        }        
        Vector3 newPosition = new Vector3(xOffset, yOffset, zOffset);
        transform.position = player.position + newPosition;        
    }

    void LandingOnTree(float landingHeight)
    {
        // Calculate the new position to land on a tree
        Vector3 treePosition = treeChange.getNearestTreePlayer() + new Vector3(0,landingHeight,0);
        if(treePosition != Vector3.zero) 
        {
            if(transform.position == treePosition) // If the bird is already on the tree
            {
                isLanded = true;
                //isLandingOff = true;
            }
            else // moves until bird on the tree
            {
                transform.position = Vector3.MoveTowards(transform.position, treePosition, 10f * Time.deltaTime);
                isLanded = false;
                isLandingOff = false;
            }
        }   
    }
    void LandingOffTree()
    {
        Vector3 posBeforeLanding;
        posBeforeLanding = player.position + new Vector3(xOffset,yOffset, zOffset);
        if(Mathf.Abs(posBeforeLanding.y - transform.position.y) <= 1f ) // If the bird is already off the tree
        {
            isLandingOff = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, posBeforeLanding, 10f * Time.deltaTime);
            isLandingOff = false;
        }
        
    }

}