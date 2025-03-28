using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private CameraTest cameraTest;

    [SerializeField] private AudioSource audioSource;

    public GameObject pauseMenuUI;

    [SerializeField] GameObject resumeButton;

    [SerializeField] GameObject mainMenuButton;

    [SerializeField] GameObject quitButton;

    [SerializeField] GameObject optionsButton;

    [SerializeField] GameObject mainMenuUI;

    [SerializeField] private Animator transition;

    private void Start()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(Resume);
        quitButton.GetComponent<Button>().onClick.AddListener(Quit);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(BackToMainMenu);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        audioSource.volume = 0.2f;
        cameraTest.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        resumeButton.SetActive(true);
        mainMenuButton.SetActive(true);
        quitButton.SetActive(true);
        optionsButton.SetActive(true);
        mainMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        audioSource.volume = 0f;
        cameraTest.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        resumeButton.SetActive(true);
        mainMenuButton.SetActive(true);
        quitButton.SetActive(true);
        optionsButton.SetActive(true);

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        StartCoroutine(BackToMainMenuCoroutine());
    }

    private IEnumerator BackToMainMenuCoroutine()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(1);
    }
}
