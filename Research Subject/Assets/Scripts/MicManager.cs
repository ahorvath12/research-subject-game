using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MicManager : MonoBehaviour
{
    public int sampleWindow = 64;
    public float loudnessSensitivity = 100;
    public float threshold = 0.1f;

    private AudioClip microphoneClip;

    public UnityEvent micTriggerEvent;
    
    void Start()
    {
        MicrophoneToAudioClip();
    }

    void Update()
    {
        float loudness = GetLoudnessFromMic() * loudnessSensitivity;

        if (loudness > threshold) {
            Debug.Log("detected " + loudness);
            micTriggerEvent.Invoke();
        }
    }

    public void MicrophoneToAudioClip() {
        string microphoneName = Microphone.devices[GameController.Instance.gameSettings.micIndex];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMic() {
        int clipPosition = Microphone.GetPosition(Microphone.devices[GameController.Instance.gameSettings.micIndex]);
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
