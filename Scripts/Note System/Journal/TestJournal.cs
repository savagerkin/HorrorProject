using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestJournal : MonoBehaviour
{
    [SerializeField] private RawImage Image; // The RawImage whose color you want to change
    [SerializeField] private Texture mainTexture; // The texture you want to display
    [SerializeField] private Texture journalTexture;
    [SerializeField] private CameraTest camera;
    private float lerpTime = 0;
    [SerializeField] private NewBehaviourScript book;
    private bool noteBookOpen = false;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        prevButton.SetActive(false);
        nextButton.SetActive(false);
        if (Image == null)
        {
            Debug.LogError("RawImage is not assigned in the inspector");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !noteBookOpen)
        {
            StopAllCoroutines();
            camera.CameraUnlocked = false;
            noteBookOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            lerpTime = 0;
            StartCoroutine(ToBlack());
        }
        else if ((Input.GetKeyDown(KeyCode.J) && noteBookOpen) || (Input.GetKeyDown(KeyCode.Escape) && noteBookOpen))
        {
            StopAllCoroutines();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camera.CameraUnlocked = true;
            Time.timeScale = 1;
            noteBookOpen = false;
            lerpTime = 0;
            StartCoroutine(ToBlack());
        }
    }

    IEnumerator ToBlack()
    {
        lerpTime = 0;
        if (noteBookOpen == false)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(false);
        }

        while (Image.color != Color.black)
        {
            lerpTime += Time.fixedUnscaledDeltaTime;
            Image.color = Color.Lerp(Image.color, Color.black, lerpTime);
            yield return null;
        }

        if (noteBookOpen == true)
        {
            Image.texture = journalTexture;
        }
        else
        {
            Image.texture = mainTexture;
        }

        lerpTime = 0;
        StartCoroutine(ToWhite());
    }

    IEnumerator ToWhite()
    {
        lerpTime = 0;

        while (Image.color != Color.white)
        {
            lerpTime += Time.fixedUnscaledDeltaTime;
            Image.color = Color.Lerp(Image.color, Color.white, lerpTime);
            yield return null;
        }

        if (noteBookOpen == true && book.firstPage)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if (noteBookOpen == true && book.lastPage)
        {
            prevButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else if (noteBookOpen == true)
        {
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }
}