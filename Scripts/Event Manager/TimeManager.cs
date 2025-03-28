using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int totalMinutesPassed { get; private set; }

    [SerializeField] private float minuteToRealTime = 0.25f;
    private float timer;
    [SerializeField] private int startingHour =11;
    void Start()
    {
        totalMinutesPassed = 0;
        Minute = 0;
        Hour = startingHour;
        timer = minuteToRealTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Minute++;
            totalMinutesPassed++;
            OnMinuteChanged?.Invoke();
            if(Minute >= 60)
            {
                Minute = 0;
                Hour++;
            }
            timer = minuteToRealTime;
        }
    }
}
