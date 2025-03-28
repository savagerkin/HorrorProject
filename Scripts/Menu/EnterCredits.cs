using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCredits : MonoBehaviour
{

    [SerializeField] private Animator transition;

    void Start ()
    {
        StartCoroutine(EnterCreditsCoroutine());
    }

    private IEnumerator EnterCreditsCoroutine()
    {
        yield return new WaitForSeconds(2f);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
