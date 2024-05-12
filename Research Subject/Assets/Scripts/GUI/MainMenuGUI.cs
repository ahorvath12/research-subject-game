using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("IntroScreens")]
    public CanvasGroup introGroup;
    public CanvasGroup introText;
    public CanvasGroup introMicSettings;
    public CanvasGroup introConfirmButton;
    public CanvasGroup introHeadphonesText;

    [Header("MenuPanels")]
    public CanvasGroup mainMenu;
    public CanvasGroup intro;
    public CanvasGroup settings;
    public CanvasGroup blackPanel;
    public List<CanvasGroup> credits;

    [Header("Other")]
    public GameObject player;
    public GameObject mainCamera;
    public GameObject loadingCamera;

    private int signatures = 0;
    public GameObject formContinueButton;

    private GuiFadeManager fadeManager;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start() {
        fadeManager = GuiFadeManager.Instance;

        fadeManager.QueueFade(new List<FadingUI> { new FadingUI(introText, 1, null, 1f), new FadingUI(introMicSettings, 1, null, 1), new FadingUI(introConfirmButton, 1) });
    }

    private void Update() {
    }

    public void Changestate(MainMenuState newState) {
        this.state = newState;
        HandleState();
    }

    private void HandleState() {
        switch(this.state) {
            case MainMenuState.INTRO:
                fadeManager.QueueFade(new List<FadingUI> { new FadingUI(intro, 1) }, 0.25f);
                break;
            default:
                break;
        }
    }

    public void ShowMainMenu()
    {
        List<FadingUI> fadingList = new List<FadingUI>();
        fadingList.Add(new FadingUI(introGroup, 0, null, 0.25f));
        fadingList.Add(new FadingUI(introHeadphonesText, 1, null, 2));
        fadingList.Add(new FadingUI(introHeadphonesText, 0, null, 0.5f));
        fadingList.Add(new FadingUI(blackPanel, 0));
        fadeManager.QueueFade(fadingList);
    }

    public void ShowForm()
    {
        float timer = 0.25f;
        fadeManager.QueueFade(new List<FadingUI>{new FadingUI(mainMenu, 0)}, timer);
        StartCoroutine(WaitAndAnimatePlayer(timer * 2));
    }

    public void LoadGame()
    {
        fadeManager.QueueFade(new List<FadingUI> { new FadingUI(intro, 0), new FadingUI(blackPanel, 1, ChangeCameraAndLoadScene) });
    }

    public void OpenSettings() {
        List<FadingUI> fadeList = new List<FadingUI>{new FadingUI(mainMenu, 0), new FadingUI(settings, 1)};
        fadeManager.QueueFade(fadeList, 0.25f);
    }

    public void CloseSettings() {
        List<FadingUI> fadeList = new List<FadingUI>{new FadingUI(settings, 0), new FadingUI(mainMenu, 1)};
        fadeManager.QueueFade(fadeList, 0.25f);
    }

    public void OpenCredits() {
        List<FadingUI> fadeList = new List<FadingUI>{new FadingUI(mainMenu, 0)};
        for (int i = 0; i < credits.Count; i++) {
            CanvasGroup cg = credits[i];
            FadingUI fadeInUI = new FadingUI(cg, 1, null, 2);
            FadingUI fadeOutUI = new FadingUI(cg, 0, null, 0.5f);
            fadeList.Add(fadeInUI);
            fadeList.Add(fadeOutUI);
        }
        fadeList.Add(new FadingUI(mainMenu, 1));
        fadeManager.QueueFade(fadeList, 0.25f);
    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void Signed() {
        signatures++;
        if (signatures >= 2) {
            fadeManager.QueueFade(new List<FadingUI>{new FadingUI(formContinueButton.GetComponent<CanvasGroup>(), 1)}, 0.3f);
        }
    }


    private void ChangeCameraAndLoadScene()
    {
        mainCamera.SetActive(false);
        loadingCamera.SetActive(true);
        fadeManager.QueueFade(new List<FadingUI> { new FadingUI(blackPanel, 0, StartLoadScene) }, 0.3f);
    }

    private void StartLoadScene()
    {
        StartCoroutine(LoadSceneAsync());
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
                yield return new WaitForSeconds(1);
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
