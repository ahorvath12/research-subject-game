using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuiManager : MonoBehaviour
{
    public GameObject mainGameUI;
    public GameObject surveyUI;

    private GameController _gameController;
    private UIState _state;

    void Start()
    {
        _gameController = GameController.Instance;
        surveyUI.SetActive(false);
        mainGameUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != _gameController.uiState) {
            _state = _gameController.uiState;

            switch(_state) {
                case UIState.START_VIEW_ROOM:
                    surveyUI.SetActive(false);
                    break;
                case UIState.VIEW_ROOM:
                    mainGameUI.SetActive(true);
                    break;
                case UIState.START_VIEW_SURVEY:
                    mainGameUI.SetActive(false);
                    break;
                case UIState.VIEW_SURVEY:
                    surveyUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
