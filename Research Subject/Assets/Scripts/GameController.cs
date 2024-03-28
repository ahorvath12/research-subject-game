using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    NOT_STARTED,
    START_VIEW_ROOM,
    VIEW_ROOM,
    START_VIEW_SURVEY,
    VIEW_SURVEY,
    PAUSE,
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameState state = GameState.VIEW_ROOM;
    public GameSettings gameSettings;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    public void HoverSubscriber(HoverType hoverType) {
        switch(hoverType) {
            case HoverType.CAMERA_BOTTOM:
                ChangeState(GameState.START_VIEW_SURVEY);
                break;
            case HoverType.SURVEY_TOP:
                ChangeState(GameState.START_VIEW_ROOM);
                break;
            default:
                break;
        }
    }

    public void ChangeState(GameState newState) {
        state = newState;
    }
}
