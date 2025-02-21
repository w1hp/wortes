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
            // Lista rozdzielczości uporządkowana od najwyższej do najniższej
            List<string> availableResolutions = new List<string>()
            {
                "2048 x 1152",  // 2K
                "1920 x 1080",  // Full HD (1080p)
                "1600 x 900",   // HD+
                "1366 x 768",   // HD
                "1280 x 720",   // HD
                "1280 x 1024",  // SXGA
                "1280 x 960",   // XGA
                "1280 x 800",   // WXGA
                "1280 x 768",   // WXGA
                "1024 x 768",   // XGA
                "800 x 600"     // SVGA
            };

            // Dodajemy rozdzielczości do dropdowna
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(availableResolutions);

            // Ustaw domyślną rozdzielczość (np. 1920x1080)
            resolutionDropdown.value = availableResolutions.IndexOf("1920 x 1080");
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
        // Lista rozdzielczości (tym razem z konkretnymi wartościami)
        var resolutionList = new List<Resolution>
        {
            new Resolution { width = 2048, height = 1152 },
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1280, height = 1024 },
            new Resolution { width = 1280, height = 960 },
            new Resolution { width = 1280, height = 800 },
            new Resolution { width = 1280, height = 768 },
            new Resolution { width = 1024, height = 768 },
            new Resolution { width = 800, height = 600 }
        };

        if (resolutionIndex >= 0 && resolutionIndex < resolutionList.Count)
        {
            Resolution selectedResolution = resolutionList[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        if (Screen.fullScreen != isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }
    }
}
