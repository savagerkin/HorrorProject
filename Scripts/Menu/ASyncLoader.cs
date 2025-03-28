using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject crossFade;
    [SerializeField] private Animator transition;
    [SerializeField] private Slider loadingSlider;

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);
        mainMenu.SetActive(false);
        PlayerPrefs.SetInt("sekcoFadeIndex", 1);
        transition.SetTrigger("End");
        yield return new WaitForSecondsRealtime(2f);
        
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if(operation.progress >= 0.9f)
            {
                transition.SetTrigger("Start");
                yield return new WaitForSecondsRealtime(2f);
                operation.allowSceneActivation = true;
                
            }
            yield return null;
        }       
    }
}
