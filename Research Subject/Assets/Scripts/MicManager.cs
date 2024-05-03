using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MicManager : MonoBehaviour
{
    public int sampleWindow = 64;
    public float loudnessSensitivity = 100;
    public float threshold = 0.5f;

    private AudioClip microphoneClip;

    [Header("Mic UI")]
    public Image icon;
    public Color offColor;
    public Color onColor;

    public UnityEvent micTriggerEvent;
    
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        float loudness = GetLoudnessFromMic() * PlayerPrefs.GetFloat("micSensitivity", 100);

        if (loudness > threshold) {
            Debug.Log("detected " + loudness);
            micTriggerEvent.Invoke();
            icon.color = onColor;
        }
        else {
            icon.color = offColor;
        }
    }

    public void MicrophoneToAudioClip() {
        string microphoneName = Microphone.devices[PlayerPrefs.GetInt("micIndex", 0)];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMic() {
        int clipPosition = Microphone.GetPosition(Microphone.devices[PlayerPrefs.GetInt("micIndex", 0)]);
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0) {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        microphoneClip.GetData(waveData, startPosition);

        // compute loudness
        float totalLoudness = 0;
        for(int i = 0; i < sampleWindow; i++) {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
 }
