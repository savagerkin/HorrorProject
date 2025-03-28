using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFall : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    bool treeFall = false;
    void TimeCheck()
    {
        if (TimeManager.Hour == 11 && TimeManager.Minute == 30)
        {
            treeFall = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(treeFall && transform.rotation.x < 90)
        {
            transform.Rotate(Time.deltaTime * 10, 0, 0);
        }

    }
}