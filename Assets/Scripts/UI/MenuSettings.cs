//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class MenuController : MonoBehaviour
//{
//    [Header("Gameplay Settings")]
//    [SerializeField] private TMP_Text controllerSenTextValue = null;
//    [SerializeField] private Slider controllerSenSlider = null;
//    [SerializeField] private float defaultSen = 4;
//    public int mainControllerSen = 4;

//    [Header("Toggle Settings")]
//    [SerializeField] private Toggle invertYToggle = null;

//    [Header("Graphics Settings")]
//    [SerializeField] private Slider brightnessSlider = null;
//    [SerializeField] private TMP_Text brightnessTextValue = null;
//    [SerializeField] private float defaultBrightness = 1;

//    private int _qualityLevel;
//    private bool _isFullScreen;
//    private float _brightnessLevel;


//    [Header("Resolution Dropdowns")]
//    public Dropdown resolutionDropdown;
//    private Resolution[] resolutions;
//    private void Start()
//    {
//        resolutions = Screen.resolutions;
//        resolutionDropdown.ClearOptions();

//        List<string> options = new List<string>();

//        int currentResolutionIndex = 0;

//        for (int i = 0; i < resolutions.Length; i++)
//        {
//            string option = resolutions[i].width + " x " + resolutions[i].height;
//            options.Add(option);

//            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
//            {
//                currentResolutionIndex = i;
//            }
//        }
//    }

//    public void SetResolution(int resolutionIndex)
//    {
//        Resolution resolution = resolutions[resolutionIndex];
//        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
//    }

//    public void SetBrightness(float brightness)
//    {
//        _brightnessLevel = brightness;
//        brightnessTextValue.text = brightness.ToString("0.0");
//    }

//    public void SetFullScreen(bool isFullscreen)
//    {
//        _isFullScreen = isFullscreen;
//    }

//    public void SetQuality(int qualityIndex)
//    {
//        _qualityLevel = qualityIndex;
//    }
//}

