using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuiManager : MonoBehaviour
{
    public GameObject mainGameUI;
    public GameObject surveyUI;

    private GameController _gameController;
    private GameState _state;

    void Start()
    {
        _gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_state != _gameController.state) {
            _state = _gameController.state;

            switch(_state) {
                case GameState.START_VIEW_ROOM:
                    surveyUI.SetActive(false);
                    break;
                case GameState.VIEW_ROOM:
                    mainGameUI.SetActive(true);
                    break;
                case GameState.START_VIEW_SURVEY:
                    mainGameUI.SetActive(false);
                    break;
                case GameState.VIEW_SURVEY:
                    surveyUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
