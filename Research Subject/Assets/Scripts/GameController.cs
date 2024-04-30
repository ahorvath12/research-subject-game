using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIState {
    NONE,
    START_VIEW_ROOM,
    VIEW_ROOM,
    START_VIEW_SURVEY,
    VIEW_SURVEY,
    PAUSE,
}

public enum GameState {
    NOT_STARTED,
    RUNNING,
    PAUSE,
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Cursor Settings")]
    public Texture2D defaultCursorTexture;
    public Texture2D penCursorTexture;
    public Texture2D selectCursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;

    [Header("Game Settings")]
    public GameState state = GameState.NOT_STARTED;
    private GameState _lastState;
    public UIState uiState = UIState.NONE;
    private UIState _lastUiState;
    public GameSettings gameSettings;

    [Header("Debugging")]
    [SerializeField]
    private bool _runTimer = false;
    private float _timer = 120;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start() {
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    void Update() {
        if (state == GameState.PAUSE) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _lastState = state;
            _lastUiState = uiState;
            state = GameState.PAUSE;
            uiState = UIState.PAUSE;
        }
        
        if (_runTimer) {
            _timer -= Time.deltaTime;
        }

        if (_timer <= 0) {
            Debug.Log("GAME OVER");
        }
    }

    public void HoverSubscriber(HoverType hoverType) {
        switch(hoverType) {
            case HoverType.CAMERA_BOTTOM:
                ChangeUIState(UIState.START_VIEW_SURVEY);
                break;
            case HoverType.SURVEY_TOP:
                ChangeUIState(UIState.START_VIEW_ROOM);
                break;
            default:
                break;
        }
    }

    public void ChangeCursorToPen(HoverType hoverType) {
        Cursor.SetCursor(penCursorTexture, Vector2.zero, cursorMode);
    }

    public void ChangeCursorToSelect(HoverType hoverType) {
        Cursor.SetCursor(selectCursorTexture, Vector2.zero, cursorMode);
    }

    public void ChangeCursorToDefault(HoverType hoverType) {
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }
    public void ChangeCursorToDefault() {
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    public void ChangeUIState(UIState newState) {
        uiState = newState;
    }

    public void ChangeGameState(GameState newState) {
        state = newState;
    }

    public void StartVideo() {
        ChangeGameState(GameState.RUNNING);
    }

    public void UnpauseGame() {
        state = _lastState;
        uiState = _lastUiState;
    }
}
