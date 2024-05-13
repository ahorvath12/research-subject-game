using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MicManager : MonoBehaviour
{
    public AudioSource source;
    public int sampleWindow = 64;
    public float threshold = 0.5f;
    private float _prevLoudness = 0;

    private AudioClip microphoneClip;

    [Header("Mic UI")]
    public List<Image> icons = new List<Image>();
    public Color offColor;
    public Color onColor;

    private int micIndex;

    public UnityEvent newMicActivityEvent, quietMicEvent;
    
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        if (micIndex != PlayerPrefs.GetInt("micIndex", 0))
        {
            Microphone.End(Microphone.devices[micIndex]);
            MicrophoneToAudioClip();
        }

        float loudness = GetLoudnessFromMic() * PlayerPrefs.GetFloat("micSensitivity", 150);

        if (loudness > threshold)
        {
            // Debug.Log("on " + loudness);
            foreach (Image icon in icons)
            {
                icon.color = onColor;
            }

            if (_prevLoudness <= threshold)
            {
                newMicActivityEvent.Invoke();
            }
        }
        else {
            // Debug.Log("off " + loudness);
            foreach (Image icon in icons)
            {
                icon.color = offColor;
            }

            if (_prevLoudness > threshold)
            {
                quietMicEvent.Invoke();
            }
        }
        _prevLoudness = loudness;
    }

    public void MicrophoneToAudioClip() {
        micIndex = PlayerPrefs.GetInt("micIndex", 0);
        string microphoneName = Microphone.devices[micIndex];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
        source.clip = microphoneClip;
        Debug.Log("set clip");
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

        if (totalLoudness / sampleWindow == 0)
        {
            Debug.Log(totalLoudness + " " + sampleWindow + " " + Microphone.devices[PlayerPrefs.GetInt("micIndex", 0)] + PlayerPrefs.GetFloat("micSensitivity", 250));
        }
        return totalLoudness / sampleWindow;
    }
 }
