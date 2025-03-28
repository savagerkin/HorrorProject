using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MushroomGrowth : MonoBehaviour
{
    [SerializeField] private GameObject mushroomGroup;
    [SerializeField] private int amountOfMushrooms;
    [SerializeField] private float mushroomWait;

    private void OnTriggerEnter(Collider other)
    {
        BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
        //finds the most upper, lower, left and right bounds of the box colliders
        float mostLeft = float.PositiveInfinity;
        float mostRight = float.NegativeInfinity;
        float mostFront = float.PositiveInfinity;
        float mostBack = float.NegativeInfinity;
        foreach (BoxCollider boxCollider in boxColliders)
        {
            if (boxCollider.bounds.min.x < mostLeft)
            {
                mostLeft = boxCollider.bounds.min.x;
            }

            if (boxCollider.bounds.max.x > mostRight)
            {
                mostRight = boxCollider.bounds.max.x;
            }

            if (boxCollider.bounds.min.z < mostFront)
            {
                mostFront = boxCollider.bounds.min.z;
            }

            if (boxCollider.bounds.max.z > mostBack)
            {
                mostBack = boxCollider.bounds.max.z;
            }
        }

        StartCoroutine(spawnMushroom(mostBack, mostFront, mostLeft, mostRight));
    }

    [SerializeField] private float randomMinSize;
    [SerializeField] private float randomMaxSize;
    IEnumerator spawnMushroom(float back, float front, float left, float right)
    {
        int i = 0;
        while (i < amountOfMushrooms)
        {
            yield return new WaitForSeconds(mushroomWait);
            i++;
            float x = UnityEngine.Random.Range(back, front);
            float z = UnityEngine.Random.Range(left, right);

            GameObject newObject =
                Instantiate(mushroomGroup, new Vector3(z, 15f, x), Quaternion.identity) as GameObject;
            //this is kinda random since I just spawn the objects 15f above the trigger,
            //and then the raymushroom function will move the indiviual mushrooms to the correct place

            float randomScale = UnityEngine.Random.Range(randomMinSize, randomMaxSize);
            switch (randomScale) //implement random scales of the mushrooms
            {
                case 1:
                    newObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    newObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    break;
                case 3:
                    newObject.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case 4:
                    newObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                    break;
            }
        }
    }
}