using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] private RawImage imgPrefab;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float afterImageInterval = 0.1f; // Interval between afterimages
    

    private RawImage currentAfterImage;
    private float lastAfterImageTime;
    private int width, height;

    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    void Update()
    {
        StartCoroutine(AfterImageEffectCoroutine());
        if (Time.time - lastAfterImageTime > afterImageInterval)
        {
            lastAfterImageTime = Time.time;
        }
    }

    IEnumerator AfterImageEffectCoroutine()
    {
        yield return new WaitForEndOfFrame();

        // Reuse or create a new Texture2D only when needed
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        if(currentAfterImage != null)
        {
            Destroy(currentAfterImage.gameObject);
        }

        currentAfterImage = Instantiate(imgPrefab, transform);
        currentAfterImage.texture = tex;
        currentAfterImage.color = new Color(1, 1, 1, 1);
        //currentAfterImage.material = afterImageMaterial;

        StartCoroutine(FadeOutAfterImage(currentAfterImage));

    }

    IEnumerator FadeOutAfterImage(RawImage afterImage)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (afterImage == null) 
                yield break;

            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            afterImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        if (afterImage != null)
        {
            Destroy(afterImage.gameObject);
        }
    }
}
