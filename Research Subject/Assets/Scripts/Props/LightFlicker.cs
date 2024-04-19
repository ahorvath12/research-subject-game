using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light light;
    public bool flicker = false;

    private float flickerWaitTime;
    private float lastFlickerTime;
    private bool isFlickering = false;
    private int maxFlickers;
    private int countFlickers = 0;
    
    void Start() {
        maxFlickers = Random.Range(3, 5);
        if (maxFlickers % 2 == 1) { // make number of flickers odd so last flicker turns off light
            maxFlickers++;
        }
    }

    void Update()
    {
        if (flicker && !isFlickering && countFlickers <= maxFlickers) {
            isFlickering = true;
            flickerWaitTime = Random.Range(0.05f, 0.2f);
            lastFlickerTime = Time.time;
            countFlickers++;
        }
        else if (isFlickering) {
            float checkTime = Time.time - lastFlickerTime;
            if (checkTime >= flickerWaitTime) {
                light.intensity = light.intensity == 1 ? 0 : 1;
                isFlickering = false;
            }
        }
    }

    private void TriggerLightFlicker() {
        flicker = true;
    }
}
