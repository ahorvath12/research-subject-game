using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGuiManager : MonoBehaviour
{
    [Header("UI Groups")]
    public GameObject mainUI;
    public GameObject pauseUI;
    public CanvasGroup blackPanel;

    [Header("Pause UI")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Game End UI")]
    public CanvasGroup gameWinUI;
    public CanvasGroup gameWinButton;
    public CanvasGroup gameLoseUI;
    public CanvasGroup gameLoseButton;

    [Header("Arrow Groups")]
    public GameObject mainGameUI;
    public GameObject surveyUI;

    [Header("Other")]
    public GameObject tvScreen;

    private GameController _gameController;
    private UIState _state;

    private GuiFadeManager fadeManager;

    void Start()
    {
        _gameController = GameController.Instance;
        surveyUI.SetActive(false);
        mainGameUI.SetActive(false);

        fadeManager = GuiFadeManager.Instance;

        fadeManager.QueueFade(new List<FadingUI>{new FadingUI(blackPanel, 0)});
    }

    void Update()
    {
        
        if (_state != _gameController.uiState) {
            if (_state == UIState.PAUSE)
            {
                pauseUI.SetActive(false);
                mainUI.SetActive(true);
            }
            _state = _gameController.uiState;

            switch(_state) {
                case UIState.START_VIEW_ROOM:
                    surveyUI.SetActive(false);
                    break;
                case UIState.VIEW_ROOM:
                    mainGameUI.SetActive(true);
                    FadeInMainGameUI();
                    break;
                case UIState.START_VIEW_SURVEY:
                    mainGameUI.SetActive(false);
                    break;
                case UIState.VIEW_SURVEY:
                    surveyUI.SetActive(true);
                    break;
                case UIState.PAUSE:
                    mainUI.SetActive(false);
                    pauseUI.SetActive(true);
                    break;
                case UIState.GAME_END:
                    fadeManager.QueueFade(new List<FadingUI> { new FadingUI(mainUI.GetComponent<CanvasGroup>(), 0) });
                    HandleGameEnd();
                    break;
                default:
                    break;
            }
        }
    }

    public void FadeInMainGameUI() {
        if (_gameController.state != GameState.NOT_STARTED) {
            return;
        }

        CanvasGroup mainGameUIGroup = mainGameUI.GetComponent<CanvasGroup>();
        if (!mainGameUIGroup) {
            return;
        }
        fadeManager.QueueFade(new List<FadingUI>{new FadingUI(mainGameUIGroup, 1, StartGame)});
    }

    public void StartGame() {
        _gameController.ChangeGameState(GameState.RUNNING);
    }

    public void ResumeGame() {
        GameController.Instance.UnpauseGame();
    }

    public void OpenSettings() {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings() {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ReturnToMenu() {
        List<FadingUI> fadeList = new List<FadingUI>();
        if (_state == UIState.GAME_END)
        {
            fadeList.Add(new FadingUI(gameWinUI, 0));
            fadeList.Add(new FadingUI(gameLoseUI, 0));
        }
        fadeList.Add(new FadingUI(blackPanel, 1, LoadMenuScene));
        fadeManager.QueueFade(fadeList, 0.25f);
    }

    private void LoadMenuScene() {
        SceneManager.LoadScene(0);
    }

    private void HandleGameEnd()
    {
        List<FadingUI> fadeList = new List<FadingUI>();

        if (GameController.Instance.state == GameState.GAME_WIN)
        {
            tvScreen.GetComponent<TvScreen>().TurnOff();
            fadeList.Add(new FadingUI(blackPanel, 1, _gameController.ChangeCamera, 0.5f));
            fadeList.Add(new FadingUI(blackPanel, 0));
            fadeList.Add(new FadingUI(gameWinUI, 1, null, 1));
            fadeList.Add(new FadingUI(gameWinButton, 1));
        }
        else
        {
            fadeList.Add(new FadingUI(blackPanel, 1, _gameController.ChangeCamera, 0.5f));
            fadeList.Add(new FadingUI(blackPanel, 0));
            fadeList.Add(new FadingUI(gameLoseUI, 1, null, 1));
            fadeList.Add(new FadingUI(gameLoseButton, 1));
        }

        fadeManager.QueueFade(fadeList, 1);
    }
}
