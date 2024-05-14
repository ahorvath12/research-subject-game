using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance {get; private set;}

    public TMP_Dropdown micDropdown;
    public Slider micSlider;
    public Slider volumeSlider;
    private int _micListCount = 0;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateMicList();

        micSlider.maxValue = 300;
        micSlider.value = PlayerPrefs.GetFloat("micSensitivity", micSlider.maxValue / 2);

        if (volumeSlider)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume", volumeSlider.maxValue);
        }

    }

    void Update() {
        if (_micListCount != Microphone.devices.Length) {
            UpdateMicList();
        }
        if (PlayerPrefs.GetFloat("micSensitivity") != micSlider.value) {
            micSlider.value = PlayerPrefs.GetFloat("micSensitivity");
        }
    }

    private void UpdateMicList() {
        string[] micList = Microphone.devices;
        micDropdown.ClearOptions();
        micDropdown.AddOptions(new List<string>(micList));
        micDropdown.value = PlayerPrefs.GetInt("micIndex", 0);
        _micListCount = micList.Length;
    }

    public void UpdateActiveMic() {
        PlayerPrefs.SetInt("micIndex", micDropdown.value);
    }

    public void UpdateMicSensitivity()
    {
        PlayerPrefs.SetFloat("micSensitivity", micSlider.value);
    }

    public void UpdateVolume() {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }
}
