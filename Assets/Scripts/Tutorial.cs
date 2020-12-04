using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Tutorial : MonoBehaviour
{

    [SerializeField] PlayerMovement player;

    [SerializeField] Slider musicSlider = default;
    [SerializeField] AudioSource[] sources;

    [SerializeField] TMP_Dropdown resolutionDropdown = default;
    Resolution[] resolutions;
    int currentResolutionIndex;

    [SerializeField] Toggle fullScreen = default;

    [SerializeField] GameObject startGameIcon;


    bool allowStart;

    private void Awake()
    {
        PlayerPrefs.SetInt("pantallas", 0);
        PlayerPrefs.SetInt("correctAnswers", 0);

        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                if ((resolutions.Length > 10 && i > resolutions.Length - 10) || resolutions.Length <= 10)
                {
                    string option = resolutions[i].width + "x" + resolutions[i].height;
                    options.Add(option);

                    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = i;
                    }
                }
            }

            resolutions.Reverse();
            resolutionDropdown.AddOptions(options);

            resolutionDropdown.value = currentResolutionIndex;

            resolutionDropdown.RefreshShownValue();
        }

        musicSlider.value = 0.3f;

        fullScreen.isOn = false;

    }

    private void Start()
    {
        Invoke("ChangeAllow", 2f);
    }

    private void Update()
    {
        if (allowStart && Input.GetKeyDown(KeyCode.Return))
        {
            player.CanMove = true;
            gameObject.SetActive(false);
        }
    }

    void ChangeAllow()
    {
        allowStart = true;
        startGameIcon.SetActive(true);
    }

    public void SetMusicVolume(float sliderValue)
    {
        foreach (AudioSource source in sources)
        {
            source.volume = sliderValue;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (Screen.fullScreen)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }


}
