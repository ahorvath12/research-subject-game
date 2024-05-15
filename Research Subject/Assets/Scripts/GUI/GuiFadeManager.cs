using System;
using System.Collections.Generic;
using UnityEngine;

public struct FadingUI {
    public CanvasGroup ui;
    public float targetAlpha;
    public Action callbackOnEnd;
    public float waitTimeAfterFade;

    public FadingUI(CanvasGroup ui, float targetAlpha, Action callbackOnEnd = null, float waitTimeAfterFade = 0 ) {
        this.ui = ui;
        this.targetAlpha = targetAlpha;
        this.callbackOnEnd = callbackOnEnd;
        this.waitTimeAfterFade = waitTimeAfterFade;
    }
}

public class GuiFadeManager : MonoBehaviour
{
    public static GuiFadeManager Instance  { get; private set; }

    private List<FadingUI> uiToFade = new List<FadingUI>();
    private int fadingIndex = 0;
    [SerializeField] private float lerpStep = 5;
    private float waitTimer = 0f;

    void Awake()
    {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Update()
    {
        if (waitTimer > 0) {
            waitTimer -= Time.deltaTime;
            return;
        }

        if (uiToFade.Count > 0) {
            CanvasGroup currentFading = uiToFade[fadingIndex].ui;
            float targetAlpha = uiToFade[fadingIndex].targetAlpha;
            currentFading.alpha = Mathf.Lerp(currentFading.alpha, targetAlpha, lerpStep * Time.deltaTime);
            if (Mathf.Abs(currentFading.alpha-targetAlpha) < 0.01) {
                currentFading.alpha = targetAlpha;
                waitTimer = 0.5f;

                if (targetAlpha == 0) {
                    currentFading.interactable = false;
                    currentFading.blocksRaycasts = false;
                }
                else {
                    currentFading.interactable = true;
                    currentFading.blocksRaycasts = true;
                }

                Action callback = uiToFade[fadingIndex].callbackOnEnd;
                if (callback != null) {
                    callback();
                }

                waitTimer = uiToFade[fadingIndex].waitTimeAfterFade;
                fadingIndex++;
            }

            if (fadingIndex >= uiToFade.Count) {
                fadingIndex = 0;
                waitTimer = 0;
                uiToFade.Clear();
            }
        }
    }

    public void QueueFade(List<FadingUI> fadingUI, float timer = 0) {
        waitTimer = timer;
        foreach(FadingUI ui in fadingUI) {
            uiToFade.Add(ui);
        }
    }
}
