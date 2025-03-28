/*
#pragma strict
#pragma downcast

private var paryTrees : TreeInstance[];
private var pvecTerrainPosition : Vector3;
private var pvecTerrainSize : Vector3;
private var pgobTreeCollide : GameObject;
private var pvecCollideScale : Vector3;
private var pbooCollideWithTrees : boolean = false;

function Start(){

	// Get the terrain's position
	pvecTerrainPosition = Terrain.activeTerrain.transform.position;

	// Get the terrain's size from the terrain data
	pvecTerrainSize = Terrain.activeTerrain.terrainData.size;
	// Get the tree instances
	paryTrees = Terrain.activeTerrain.terrainData.treeInstances;
	
	// Get the invisible capsule having the capsule collider that makes the nearest tree solid
	pgobTreeCollide = GameObject.Find("Tree");		// This is a capsule having a capsule collider, but when the flier hits it we want it to be reported that the flier hit a tree.
	
	// Are there trees and a tree collider?
	if ((pgobTreeCollide != null) && (paryTrees.length > 0)){
		// Set a flag to make this script useful
		pbooCollideWithTrees = true;
		// Get the original local scale of the capsule.  This is manually matched to the scale of the prototype of the tree.
		pvecCollideScale = pgobTreeCollide.transform.localScale;
	}
	// No need to use this script
	else {Destroy(this);}
}

function Update () {
	var L : int;
	var triTree : TreeInstance;
	var vecFlier : Vector3 = sctFly.svecXYZ;		// My protagonist's position, passed by a static variable in a script called sctFly.
	var fltProximity : float;
	var fltNearest : float = 9999.9999;				// Farther, to start, than is possible in my game.
	var vec3 : Vector3;
	var vecTree : Vector3;
	var intNearestPntr : int;
	
	// Test the flag
	if (pbooCollideWithTrees == true){
		// Find the nearest tree to the flier
		for (L = 0; L < paryTrees.length; L++){
			// Get the tree instance
			triTree = paryTrees[L];
			// Get the normalized tree position
			vecTree = triTree.position;
			// Get the world coordinates of the tree position
			vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
			// Calculate the proximity
			fltProximity = Vector3.Distance(vecFlier, vec3);
			// Nearest so far?
			if (fltProximity < fltNearest){
				// Remember the nearest
				fltNearest = fltProximity;
				// Remember the index
				intNearestPntr = L;
			}
		}
		// Get the closest tree
		triTree = paryTrees[intNearestPntr];
		// Get the normalized tree position of the closest tree
		vecTree = triTree.position;
		// Get the world coordinates of the tree position
		vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
		// Scale the capsule having the capsule collider that represents a solid tree
		pgobTreeCollide.transform.localScale = (pvecCollideScale * triTree.heightScale);
		// Add some height to position the capsule correctly on the tree
		vec3.y += pgobTreeCollide.transform.localScale.y;
		// Position the capsule having the capsule collider at the nearest tree
		pgobTreeCollide.transform.position = vec3;
	}
}
*/