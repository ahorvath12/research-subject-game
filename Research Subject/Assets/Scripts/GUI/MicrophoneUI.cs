using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicrophoneUI : MonoBehaviour
{
    public TMP_Dropdown micDropdown;

    void Start()
    {
        string[] micList = Microphone.devices;
        micDropdown.ClearOptions();
        micDropdown.AddOptions(new List<string>(micList));
        micDropdown.value = GameController.Instance.gameSettings.micIndex;
    }

    public void UpdateActiveMic() {
        GameController.Instance.gameSettings.micIndex = micDropdown.value;
    }
}
