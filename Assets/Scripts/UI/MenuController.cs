using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider;
    private float brightnessLevel = 1f;

    [Header("UI Background Settings")]
    [SerializeField] private Image backgroundImage;  // Pole dla obrazu tła UI

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
        // Używamy wartości suwaka od 0 do 1, aby obliczyć skalę jasności.
        // Jasność tła w skali od 0.2 (ciemno) do 1.8 (bardzo jasne).
        brightnessLevel = Mathf.Lerp(0.2f, 1f, brightness);  // Zmieniamy skalę jasności

        // Zmieniamy intensywność światła (skalowanie w zakresie od 0.1 do 3)
        Light mainLight = FindObjectOfType<Light>();
        if (mainLight != null)
        {
            // Ustawiamy intensywność światła, aby rosła z wartości 0.1 do 3
            mainLight.intensity = Mathf.Lerp(0.1f, 3f, brightness);  // Jasność światła
            mainLight.color = Color.white * brightnessLevel;  // Ustawiamy kolor światła na jasność
        }

        // Zmieniamy kolor tła UI w zależności od brightnessLevel
        if (backgroundImage != null)
        {
            // Kolor tła w UI (od ciemnego do jasnego)
            Color newColor = new Color(brightnessLevel, brightnessLevel, brightnessLevel, 1f);  // Kolor tła na podstawie jasności
            backgroundImage.color = newColor;
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
