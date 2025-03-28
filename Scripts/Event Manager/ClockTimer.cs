using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTimer : MonoBehaviour
{
    void OnEnable()
    {
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }


    void TimeCheck()
    {
    }

    private int xRotation;

    private int previousMinute;
    private float xHourRotation;

    [SerializeField] private bool min;

    private bool clockOn = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (clockOn)
            {
                gameObject.transform.parent.Translate(Vector3.down * 5);
                clockOn = false;
            }
            else
            {
                gameObject.transform.parent.Translate(Vector3.up * 5);
                clockOn = true;
            }
        }

        //  Angle_mins =6 * number of minutes
        //  Angle_hours = 30 * number of hours + 0.5 * number of minutes

        xRotation = (6 * TimeManager.Minute);
        if (TimeManager.Hour < 12)
        {
            xHourRotation = 30 * TimeManager.Hour + 0.5f * TimeManager.Minute;
        }
        else
        {
            xHourRotation = 30 * (TimeManager.Hour - 12) + 0.5f * TimeManager.Minute;
        }

        if (min)
        {
            transform.localEulerAngles = new Vector3(-1 * (xRotation + 90), 0, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(-1 * (xHourRotation + 90), 0, 0);
        }
    }
}