using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

struct FadingUI {
    public CanvasGroup ui;
    public float targetAlpha;

    public Action callbackOnEnd;

    public FadingUI(CanvasGroup ui, float targetAlpha, Action callbackOnEnd = null ) {
        this.ui = ui;
        this.targetAlpha = targetAlpha;
        this.callbackOnEnd = callbackOnEnd;
    }
}

public enum MainMenuState {
    MAIN,
    INTRO,
    SETTINGS,
    CREDITS,
}

public class MainMenuGUI : MonoBehaviour
{
    public static MainMenuGUI Instance { get; private set; }
    public MainMenuState state = MainMenuState.MAIN;

    [Header("MenuPanels")]
    public CanvasGroup mainMenu;
    public CanvasGroup intro;
    public CanvasGroup blackPanel;

    [Header("Other")]
    public GameObject player;

    private int signatures = 0;
    public GameObject formContinueButton;
    private bool canShowContinueButton;


    private List<FadingUI> uiToFade = new List<FadingUI>();
    private int fadingIndex = 0;
    [SerializeField] private float lerpStep = 5;
    private float waitTimer = 0f;

    private bool activateGameScene = false;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    private void Update() {
        if (waitTimer > 0) {
            waitTimer -= Time.deltaTime;
            return;
        }

        if (uiToFade.Count > 0) {
            CanvasGroup currentFading = uiToFade[fadingIndex].ui;
            float targetAlpha = uiToFade[fadingIndex].targetAlpha;
            currentFading.alpha = Mathf.Lerp(currentFading.alpha, targetAlpha, lerpStep * Time.deltaTime);
            if (Mathf.Abs(currentFading.alpha-targetAlpha) < 0.05) {
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
                fadingIndex++;
            }

            if (fadingIndex >= uiToFade.Count) {
                fadingIndex = 0;
                waitTimer = 0;
                uiToFade.Clear();
            }
        }
    }

    public void Changestate(MainMenuState newState) {
        this.state = newState;

        HandleState();
    }

    private void HandleState() {
        switch(this.state) {
            case MainMenuState.INTRO: 
                waitTimer = 0.25f;
                uiToFade.Add(new FadingUI(intro, 1, StartLoadScene));
                break;
            default:
                break;
        }
    }

    public void LoadGame() {
        waitTimer = 0.25f;
        uiToFade.Add(new FadingUI(mainMenu, 0));
        StartCoroutine(WaitAndAnimatePlayer(waitTimer * 2));
    }

    public void StartGame() {
        uiToFade.Add(new FadingUI(blackPanel, 1, ChangeScene));
    }

    public void OpenSettings() {

    }

    public void OpenCredits() {

    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void Signed() {
        signatures++;
        if (signatures >= 2) {
            uiToFade.Add(new FadingUI(formContinueButton.GetComponent<CanvasGroup>(), 1));
            waitTimer = 0.3f;
        }
    }


    // ACTIONS
    public void StartLoadScene() {
        StartCoroutine(LoadSceneAsync());
    }

    public void ChangeScene() {
        activateGameScene = true;
    }

    // SCENE FUNCTIONALITY
    private IEnumerator LoadSceneAsync() {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                canShowContinueButton = true;
                if (activateGameScene)
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator WaitAndAnimatePlayer(float timer) {
        yield return new WaitForSeconds(timer);
        player.GetComponent<Animator>().enabled = true;
    }
}
