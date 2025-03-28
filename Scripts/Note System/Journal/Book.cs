using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages;
    public bool lastPage = false;
    public bool firstPage = false;
    int index = -1;
    bool rotate = false;
    [SerializeField] GameObject backButton;

    [SerializeField] GameObject forwardButton;

    // Start is called before the first frame update
    private void Start()
    {
        InitialState();
    }

    private void Update()
    {
        if (index == pages.Count - 1)
        {
            lastPage = true;
        }
        else
        {
            lastPage = false;
        }

        if (index == -1)
        {
            firstPage = true;
        }else
        {
            firstPage = false;
        }
        
    }

    public void InitialState()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.rotation = Quaternion.identity;
        }

        pages[0].SetAsLastSibling();
        backButton.SetActive(false);
    }

    public void RotateNext()
    {
        if (rotate)
        {
            return;
        }

        index++;
        float angle = 180;
        ForwardButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, true));
    }

    public void ForwardButtonActions()
    {
        if (backButton.activeInHierarchy == false)
        {
            backButton.SetActive(true);
        }

        if (index == pages.Count - 1)
        {
            forwardButton.SetActive(false);
        }
    }

    public void BackButtonActions()
    {
        if (forwardButton.activeInHierarchy == false)
        {
            forwardButton.SetActive(true);
        }

        if (index - 1 == -1)
        {
            backButton.SetActive(false);
        }
    }

    public void RotateBack()
    {
        if (rotate)
        {
            return;
        }

        float angle = 0;
        BackButtonActions();
        pages[index].SetAsLastSibling();

        StartCoroutine(Rotate(angle, false));
    }

    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        while (true)
        {
            rotate = true;
            // Change the sign of the angle to reverse the rotation direction
            Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);
            value += Time.fixedUnscaledDeltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (forward == false)
                {
                    index--;
                }

                rotate = false;
                break;
            }

            yield return null;
        }
    }
}