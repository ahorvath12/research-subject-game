using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UIState {
    NONE,
    START_VIEW_ROOM,
    VIEW_ROOM,
    START_VIEW_SURVEY,
    VIEW_SURVEY,
    GAME_END,
    PAUSE,
}

public enum GameState {
    NOT_STARTED,
    RUNNING,
    GAME_WIN,
    GAME_LOSE,
    PAUSE,
}

class EventBlock
{
    public bool hasPropEvent = false;
    public bool hasTvEvent = false;
    public bool hasSfxEvent = false;
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

    [Header("Other")]
    public GameObject mainCam;
    public GameObject gameWinCam;
    public GameObject gameLoseCam;
    [HideInInspector] public UnityEvent pauseEvent, unpauseEvent;

    [Header("Debugging")]
    [SerializeField]
    private bool _runTimer = false;
    [HideInInspector] public float maxTime = 120;
    public float timer = 0;

    private float _eventsStartTime = 10;
    private float _mediumEventsStartTime = 30;
    private bool _startEvents, _startMediumEvents;
    [HideInInspector] public float monsterStartTime = 52;

    private List<DisturbanceEvent> _disturbances = new List<DisturbanceEvent>();
    private List<EventBlock> _eventTimeBlocks = new List<EventBlock>();
    private float _eventStepTime = 10;

    private bool _initialized = false;

    void Awake() {
        if (Instance == null || Instance != this) {
            Instance = this;
        }
    }

    void Start() {
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, cursorMode);
    }

    void Update() {

        if (state == GameState.NOT_STARTED && !_initialized)
        {
            InitializeEventTimes();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && state != GameState.PAUSE)
        {
            PauseGame();
            return;
        }

        if (state != GameState.RUNNING) {
            return;
        }


        if (_runTimer) {
            timer += Time.deltaTime;
        }

        if (timer >= maxTime)
        {
            _runTimer = false;
            state = GameState.GAME_WIN;
            uiState = UIState.GAME_END;
        }

        HandleTimerChecks();
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

    private void InitializeEventTimes()
    {
        if (_disturbances.Count == 0)
        {
            return;
        }
        _initialized = true;

        for (int i = 0; i < maxTime / 10; i++)
        {
            _eventTimeBlocks.Add(new EventBlock());
        }

        int beforeMonsterIndex = (int)monsterStartTime / 10;

        //prioritize adding the events that MUST occur to the time blocks
        for (int i = 0; i < _disturbances.Count; i++)
        {
            DisturbanceEvent ev = _disturbances[i];
            if (ev.mustOccur)
            {
                int maxIndex = ev.triggerBeforeMonster ? beforeMonsterIndex : _eventTimeBlocks.Count - 1;
                int randomIndex = Random.Range(1, maxIndex);
                while ((ev.type != DisturbanceType.TV && ev.type != DisturbanceType.SOUND && _eventTimeBlocks[randomIndex].hasPropEvent) ||
                        (ev.type == DisturbanceType.SOUND && _eventTimeBlocks[randomIndex].hasSfxEvent))
                {
                    randomIndex = (randomIndex + 1) % maxIndex;
                    if (randomIndex == 0)
                    {
                        randomIndex++;
                    }
                }
                if (ev.type == DisturbanceType.SOUND)
                {
                    _eventTimeBlocks[randomIndex].hasSfxEvent = true;
                }
                else
                {
                    _eventTimeBlocks[randomIndex].hasPropEvent = true;
                }
                ev.SetTimestamp(Random.Range(0, 10) + (10 * randomIndex));
            }
        }

        // add the rest of the prop events
        for (int i = 0; i < _disturbances.Count; i++)
        {
            DisturbanceEvent ev = _disturbances[i];
            if (!ev.mustOccur)
            {
                int maxIndex = ev.triggerBeforeMonster ? beforeMonsterIndex : _eventTimeBlocks.Count - 1;
                int randomIndex = Random.Range(1, maxIndex);
                int firstAttemptIndex = randomIndex;
                bool failed = false;
                while (
                    (
                        (ev.type != DisturbanceType.TV && ev.type != DisturbanceType.SOUND && _eventTimeBlocks[randomIndex].hasPropEvent) ||
                        (ev.type == DisturbanceType.TV && _eventTimeBlocks[randomIndex].hasTvEvent) ||
                        (ev.type == DisturbanceType.SOUND && _eventTimeBlocks[randomIndex].hasSfxEvent)

                    )
                    && !failed)
                {
                    int increment = ev.type == DisturbanceType.TV ? 2 : 1;
                    randomIndex = (randomIndex + 1) % maxIndex;
                    if (randomIndex == 0)
                    {
                        randomIndex++;
                    }

                    if (randomIndex == firstAttemptIndex)
                    {
                        failed = true;
                    }
                }
                if (!failed)
                {
                    if (ev.type == DisturbanceType.TV)
                    {
                        _eventTimeBlocks[randomIndex].hasTvEvent = true;
                    }
                    else if (ev.type == DisturbanceType.SOUND)
                    {
                        _eventTimeBlocks[randomIndex].hasSfxEvent = true;
                    }
                    else
                    {
                        _eventTimeBlocks[randomIndex].hasPropEvent = true;
                    }
                    ev.SetTimestamp(Random.Range(0, 10) + (10 * randomIndex));
                }
            }
        }
    }

    private void HandleTimerChecks()
    {
        if (!_runTimer)
        {
            return;
        }

        if (timer >= _mediumEventsStartTime && !_startMediumEvents)
        {
            _startMediumEvents = true;
        }
        else if (timer >= _eventsStartTime && !_startEvents)
        {
            _startEvents = true;
        }
    }


    // CALLBACKS
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

    public void PauseGame()
    {
        _lastState = state;
        _lastUiState = uiState;
        state = GameState.PAUSE;
        uiState = UIState.PAUSE;

        pauseEvent.Invoke();
    }

    public void UnpauseGame() {
        state = _lastState;
        uiState = _lastUiState;

        unpauseEvent.Invoke();
    }

    public void AddDisturbanceEvent(DisturbanceEvent e)
    {
        _disturbances.Add(e);
    }


    // subscription
    public void SubscribeToPause(UnityAction action)
    {
        pauseEvent.AddListener(action);
    }

    public void SubscribeToUnpause(UnityAction action)
    {
        unpauseEvent.AddListener(action);
    }

    public void ChangeCamera()
    {
        if (state == GameState.GAME_WIN)
        {
            mainCam.SetActive(false);
            gameWinCam.SetActive(true);
        }
        else if (state == GameState.GAME_LOSE)
        {
            mainCam.SetActive(false);
            gameLoseCam.SetActive(true);
        }
    }
}
