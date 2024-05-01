using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance {get; private set;}

    public GameSettings gameSettings;

    public TMP_Dropdown micDropdown;
    public Slider micSlider;
    public Slider volumeSlider;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start()
    {
        string[] micList = Microphone.devices;
        micDropdown.ClearOptions();
        micDropdown.AddOptions(new List<string>(micList));
        micDropdown.value = gameSettings.micIndex;

        micSlider.value = gameSettings.micSensitivity;
        volumeSlider.value = gameSettings.volume;
    }

    public void UpdateActiveMic() {
        gameSettings.micIndex = micDropdown.value;
    }

    public void UpdateMicSensitivity() {
        gameSettings.micSensitivity = micSlider.value;
    }

    public void UpdateVolume() {
        gameSettings.volume = volumeSlider.value;
    }
}
