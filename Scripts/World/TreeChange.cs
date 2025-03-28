using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeChange : MonoBehaviour
{
    [SerializeField] float TickTimer = 0.5f;
    private float _Timer = 0;
    
    [SerializeField] Terrain terrain;
    [SerializeField] GameObject treeCollider;
    [SerializeField] GameObject character;
    [SerializeField] GameObject wantedCharacter;
    
    TerrainData terrainData;
    Vector3 terrainPosition;
    Vector3 terrainSize;
    Vector3 treePosition;
    Vector3 treeColliderScale;
    Vector3 nearestTreeIndex;
    Vector3 vecPlayer;
    
    TreeInstance[] originalTrees;
    TreeInstance[] trees;
    
    bool collideWithTrees = false;
    public Vector3 getNearestTreeMonster() { return nearestTreeIndex; }
    Vector3 nearestTreePlayer;
    public Vector3 getNearestTreePlayer() { return nearestTreePlayer; }

    int treeIndex;
    
    int L;
    TreeInstance triTree;
    TreeInstance wantedTriTree;
        
    float proximity;
    float wantedObjectProximity;
    
    Vector3 vec3;
    Vector3 wantedVec3;
    Vector3 vecTree;
    Vector3 wantedVecTree;
    
    float nearest;
    float nearestwanted;
    int nearestIndex;
    int nearestWantedIndex;
    
    Vector3 vecWantedPlayer;
    

    void Start()
    {
        terrainData = terrain.terrainData;
        trees = terrainData.treeInstances;
        originalTrees = terrainData.treeInstances;
        terrainPosition = Terrain.activeTerrain.transform.position;
        terrainSize = Terrain.activeTerrain.terrainData.size;
        character = character.transform.GetChild(0).gameObject;

        if(treeCollider != null && trees.Length > 0)
        {
            collideWithTrees = true;
            treeColliderScale = treeCollider.transform.localScale;
        }
    }

    private void FixedUpdate()
    {
        _Timer += Time.deltaTime;
        if(_Timer > TickTimer)
        {
            _Timer = 0;
            Tick();
        }
    }

    public void treeChangeToMushroom()
    {
        for(int i = 0; i < trees.Length;i++)
        {
            trees[i].prototypeIndex = 1;
            trees[i].rotation = Mathf.Deg2Rad*270;
        }
        terrainData.treeInstances = trees;        
    }
    public void mushroomToNormal()
    {
        for(int i = 0; i < trees.Length;i++)
        {
            trees[i].prototypeIndex = (Random.Range(0,2) == 0) ? 0 : 2;
            trees[i].rotation = Mathf.Deg2Rad*270;
        }
        terrainData.treeInstances = trees;
        originalTrees = terrainData.treeInstances;
    }
    void Tick()
    {
        nearest = 10f;
        nearestwanted = 10f;    
        
        nearestIndex = -1;
        nearestWantedIndex = 0;
        
        if(collideWithTrees)
        {
            vecPlayer = character.transform.position;
            vecWantedPlayer = wantedCharacter.transform.position;
            //finding neares tree to the player
            for(L = 0; L < trees.Length; L++)
            {
                //getting tree instance
                triTree = originalTrees[L];
                wantedTriTree = originalTrees[L];
                //getting normalized tree position
                vecTree = triTree.position;
                wantedVecTree = wantedTriTree.position;
                
                //getting world coordinates of the tree position
                vec3 = (Vector3.Scale(terrainSize, vecTree) + terrainPosition);
                wantedVec3 = (Vector3.Scale(terrainSize,wantedVecTree) + terrainPosition);
                //calculating the proximity
                proximity = Vector3.Distance(vecPlayer, vec3);
                wantedObjectProximity = Vector3.Distance(vecWantedPlayer, wantedVec3);
                if(proximity < nearest)
                {
                    //remembering the nearest
                    nearest = proximity;
                    //remembering the index
                    nearestIndex = L;
                    treeIndex = triTree.prototypeIndex;
                }
                if(wantedObjectProximity < nearestwanted)
                {
                    //remembering the nearest
                    nearestwanted = wantedObjectProximity;
                    //remembering the index
                    nearestWantedIndex = L;
                    // treeWantedIndex = wantedTriTree.prototypeIndex;
                }
            }
            if(nearestIndex != -1)
            {
                triTree = originalTrees[nearestIndex];
                wantedTriTree = originalTrees[nearestWantedIndex];
                vecTree = triTree.position;
                wantedVecTree = wantedTriTree.position;
                vec3 = (Vector3.Scale(terrainSize, vecTree) + terrainPosition);
                wantedVec3 = (Vector3.Scale(terrainSize, wantedVecTree) + terrainPosition);
                treeCollider.transform.localScale = (treeColliderScale * triTree.heightScale);
                vec3.y += treeCollider.transform.localScale.y;
                treeCollider.transform.position = vec3;
                nearestTreePlayer = vec3;
                nearestTreeIndex = wantedVec3;
            }
        }
        
        //Debug.Log(nearestIndex);
    }
}
