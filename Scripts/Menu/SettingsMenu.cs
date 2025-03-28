using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider ambientVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider framerateSlider;
    [SerializeField] private Toggle framerateToggle;

    [SerializeField] private Slider sensitivitySlider;

    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private TMP_Text frameratePlaceHolder;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private TMP_InputField framerateInputField;


    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private float newSens;

    private Dictionary<float, string> knownAspectRatios = new Dictionary<float, string>
    {
        { 1f / 1f, "1:1" },
        { 1.85f / 1f, "1.85:1" },
        { 2f / 1f, "2:1" },
        { 2.4f / 1f, "2.4:1" },
        { 2.55f / 1f, "2.55:1" },
        { 2.76f / 1f, "2.76:1" },
        { 4f / 3f, "4:3" },
        { 5f / 4f, "5:4" },
        { 14f / 9f, "14:9" },
        { 16f / 9f, "16:9" },
        { 16f / 10f, "16:10" },
        { 21f / 9f, "21:9" } 
    };
    
    void Start () 
    {
        sensitivitySlider.minValue = 0.1f;
        sensitivitySlider.maxValue = 10f;

        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 1f);
        masterVolumeSlider.value = PlayerPrefs.GetFloat("volumeMaster", 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
        ambientVolumeSlider.value = PlayerPrefs.GetFloat("volumeAmbient", 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("volumeSFX", 0.75f);
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 2);


        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        ambientVolumeSlider.onValueChanged.AddListener(SetAmbientVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        framerateInputField.onEndEdit.AddListener(OnFPSInputChanged);
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        CheckFullscreen();
        CheckVsync();


        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        framerateToggle.isOn = PlayerPrefs.GetInt("framerateToggle", 1) > 0;
        
        framerateSlider.value = PlayerPrefs.GetFloat("framerate", 60f);
        framerateSlider.onValueChanged.AddListener(SetFramerate);

        if (framerateToggle.isOn)
        {
        SetFramerate(framerateSlider.value);
        }

        framerateToggle.onValueChanged.AddListener(ToggleFrameRateLimiter);
        ToggleFrameRateLimiter(framerateToggle.isOn);

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            float aspectRatio = (float)filteredResolutions[i].width / filteredResolutions[i].height;
            string closestKnownAspectRatio = GetClosestKnownAspectRatio(aspectRatio);
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " (" + closestKnownAspectRatio + ")";
            options.Add(option);

            if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private string GetClosestKnownAspectRatio(float aspectRatio)
    {
        float closestDifference = float.MaxValue;
        string closestKnownAspectRatio = "";

        foreach (var knownAspectRatio in knownAspectRatios)
        {
            float difference = Math.Abs(aspectRatio - knownAspectRatio.Key);
            if (difference < closestDifference)
            {
                closestDifference = difference;
                closestKnownAspectRatio = knownAspectRatio.Value;
            }
        }

        return closestKnownAspectRatio;
    }

    public void SetFramerate(float framerate)
    {
        ToggleFrameRateLimiter(true);
        Application.targetFrameRate = (int)framerate;
        PlayerPrefs.SetFloat("framerate", framerate);
        frameratePlaceHolder.text = "" + (int)framerate;
        framerateInputField.text = "" + (int)framerate;
    }

    public void ToggleFrameRateLimiter(bool isOn)
    {
        if (isOn)
        {
            Application.targetFrameRate = (int)framerateSlider.value;
            framerateToggle.GetComponent<Toggle>().isOn = true;
            frameratePlaceHolder.text = "" + (int)framerateSlider.value;
            framerateInputField.text = "" + (int)framerateSlider.value;
            PlayerPrefs.SetInt("framerateToggle", 1);
        }
        else
        {
            Application.targetFrameRate = -1; // Setting to -1 makes the game run with no framerate limit
            framerateToggle.GetComponent<Toggle>().isOn = false;
            frameratePlaceHolder.text = "Unlimited";
            framerateInputField.text = "Unlimited";
            PlayerPrefs.SetInt("framerateToggle", 0);
        }

    }
    
    public void OnFPSInputChanged(string input)
    {
        if (int.TryParse(input, out int newFPS))
        {
            if (newFPS > 500)
            {
                newFPS = 500;
                framerateInputField.text = newFPS.ToString();
                framerateSlider.value = newFPS;
            }
            else
            {
                framerateSlider.value = newFPS;
            }
        }
    }
    


    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    [SerializeField] private AudioMixer audioMixer;
    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("volumeMaster", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeMaster", volume);
    }

    public void SetMusicVolume (float volume)
    {
        audioMixer.SetFloat("volumeMusic", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeMusic", volume);
    }

    public void SetAmbientVolume (float volume)
    {
        audioMixer.SetFloat("volumeAmbient", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeAmbient", volume);
    }

    public void SetSFXVolume (float volume)
    {
        audioMixer.SetFloat("volumeSFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volumeSFX", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    public void CheckFullscreen()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void CheckVsync()
    {
        bool isVsync = PlayerPrefs.GetInt("vsync", 1) == 1;
        vsyncToggle.isOn = isVsync;
    }

    public void VsyncToggle(bool isVsync)
    {
        QualitySettings.vSyncCount = isVsync ? 1 : 0;
        PlayerPrefs.SetInt("vsync", isVsync ? 1 : 0);
    }

    public void ModifySensitivity() 
    {
        newSens = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivity", newSens);
        sensitivityText.text = newSens.ToString("F3");
    }
}
