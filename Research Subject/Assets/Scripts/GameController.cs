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

    [Header("Cursor Settings")]
    public Texture2D defaultCursorTexture;
    public Texture2D penCursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    public Color cursorColor;

    [Header("Game Settings")]
    public GameState state = GameState.VIEW_ROOM;
    public GameSettings gameSettings;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start() {
        // ColorCursor(defaultCursorTexture);
        // ColorCursor(penCursorTexture);
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
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

    public void ChangeCursorToPen(HoverType hoverType) {
        Cursor.SetCursor(penCursorTexture, Vector2.zero, cursorMode);
    }

    public void ChangeCursorToDefault(HoverType hoverType) {
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    public void ChangeState(GameState newState) {
        state = newState;
    }

    void ColorCursor(Texture2D cursorTexture)
    {
        for (int y=0; y< cursorTexture.height; y++)
        {
            for(int x = 0; x < cursorTexture.width; x++)
                cursorTexture.SetPixel(x, y, cursorColor);
        }
        cursorTexture.Apply();
    }
}
