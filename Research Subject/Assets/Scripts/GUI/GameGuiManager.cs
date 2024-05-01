using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuiManager : MonoBehaviour
{
    [Header("UI Groups")]
    public GameObject mainUI;
    public GameObject pauseUI;
    public CanvasGroup blackPanel;

    [Header("Pause UI")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Arrow Groups")]
    public GameObject mainGameUI;
    public GameObject surveyUI;

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
            if (_state == UIState.PAUSE) {
                mainUI.SetActive(true);
                pauseUI.SetActive(false);
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

    }
}
