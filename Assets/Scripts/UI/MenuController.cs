using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider;
    private float brightnessLevel = 1f;

    [Header("Resolution Settings")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        // Automatyczne przypisanie Brightness Slider, jeśli nie jest przypisany
        if (brightnessSlider == null)
        {
            brightnessSlider = GameObject.Find("Brightness_Slider")?.GetComponent<Slider>();
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        // Automatyczne przypisanie Resolution Dropdown, jeśli nie jest przypisany
        if (resolutionDropdown == null)
        {
            resolutionDropdown = GameObject.FindObjectOfType<TMP_Dropdown>();
        }

        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        // Automatyczne przypisanie Fullscreen Toggle, jeśli nie jest przypisany
        if (fullScreenToggle == null)
        {
            fullScreenToggle = GameObject.FindObjectOfType<Toggle>();
        }

        if (fullScreenToggle != null)
        {
            fullScreenToggle.isOn = Screen.fullScreen;
            fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        }
    }

    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness; // Teraz zmienna jest używana

        Light mainLight = FindObjectOfType<Light>();
        if (mainLight != null)
        {
            mainLight.intensity = Mathf.Clamp(brightnessLevel * 2f, 0.1f, 3f);
            mainLight.color = Color.white * brightnessLevel;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0)
        {
            resolutions = Screen.resolutions;
        }

        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length)
        {
            return;
        }

        Resolution resolution = resolutions[resolutionIndex];

#if !UNITY_EDITOR
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
#endif
    }

    public void SetFullScreen(bool isFullscreen)
    {
#if !UNITY_EDITOR
            if (Screen.fullScreen != isFullscreen)
            {
                Screen.fullScreen = isFullscreen;
            }
#endif
    }
}
