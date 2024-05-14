using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject head;
    public Transform playerTarget;
    private Animator _animator;

    // animation vars
    private int _step = 0;
    private int _prevStep = 0;
    private float _stepTimer;
    private bool _lookAtPlayer = false;
    private bool _playingAnimation = false;

    // noise vars
    private bool _timingNoise = false;
    private bool _timingQuiet = false;
    private float _noiseTime = 0;
    private float _quietTime = 0;
    private int _noiseLevel = 0; // goes up to 3
    private int _prevNoiseLevel = 0;
    private int _maxNoiseLevel = 4;
    private bool _canIncreaseNoiseLevel = true;

    private List<float> _noiseQueue = new List<float>();
    private int _noiseQueueMaxSize = 5;

    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();

        _stepTimer = GameController.Instance.monsterStartTime;

        GameController.Instance.SubscribeToPause(HandlePause);
        GameController.Instance.SubscribeToUnpause(HandleUnpause);
    }

    // Update is called once per frame
    void Update()
    {
        GameController gameController = GameController.Instance;

        if (gameController.state != GameState.RUNNING)
        {
            return;
        }

        if (gameController.timer >= gameController.maxTime - 5)
        {
            _animator.SetBool("end", true);
            _animator.SetInteger("step", 0);
            _animator.SetInteger("scare", 0);
            return;
        }

        if (_noiseLevel == 0)
        {
            HandleAnimation();
        }

        Debug.Log(_noiseLevel);
    }

    void LateUpdate()
    {
        if (_step <= 4)
        {
            return;
        }

        HandleNoise();

        if (_prevNoiseLevel != _noiseLevel)
        {
            HandleNoiseLevel();
        }
        if (_lookAtPlayer)
        {
            head.transform.LookAt(playerTarget);
        }
    }

    private void HandleAnimation()
    {
        if (_stepTimer > 0)
        {
            _stepTimer -= Time.deltaTime;
            return;
        }

        if (_playingAnimation)
        {
            return;
        }

        _step++;
        _animator.SetInteger("step", _step);
        _playingAnimation = true;
    }

    // NOISE FUNCTIONS

    public void InitialNoiseAction()
    {
        _timingNoise = true;
        _noiseQueue.Add(Time.time);
        if (_noiseQueue.Count > _noiseQueueMaxSize)
        {
            _noiseQueue.RemoveAt(0);
        }
        _timingQuiet = false;
        _quietTime = 0;
    }

    public void NoMoreNoiseAction()
    {
        _timingNoise = false;
        _noiseTime = 0;
        _timingQuiet = true;
    }

    private void HandleNoise()
    {
        if (!_canIncreaseNoiseLevel)
        {
            return;
        }

        _prevNoiseLevel = _noiseLevel;

        // handle constant noise
        if (_timingNoise)
        {
            _noiseTime += Time.deltaTime;

            if (_noiseTime > 0.5f && _noiseLevel < _maxNoiseLevel)
            {
                _noiseLevel++;
                _noiseTime = 0;
            }
        }

        // handle noise with short breaks between them
        if (_noiseQueue.Count == _noiseQueueMaxSize)
        {
            float averageNoiseBreakTime = 0;
            for (int i = 1; i < _noiseQueue.Count; i++)
            {
                averageNoiseBreakTime += _noiseQueue[i] - _noiseQueue[i - 1];
            }

            averageNoiseBreakTime /= _noiseQueue.Count - 1;

            if (averageNoiseBreakTime <= 0.5f && _noiseLevel < _maxNoiseLevel)
            {
                _noiseLevel++;
            }
            _noiseQueue = new List<float> { _noiseQueue[_noiseQueueMaxSize - 1] }; // reset queue
        }

        // constant quiet
        if (_timingQuiet)
        {
            _quietTime += Time.deltaTime;

            if (_quietTime > 2 && _noiseLevel > 0)
            {
                _noiseLevel--;
                _quietTime = 0;
            }
        }
    }

    private void HandleNoiseLevel()
    {
        _canIncreaseNoiseLevel = false;
        _animator.SetInteger("scare", _noiseLevel);
        switch (_noiseLevel)
        {
            case 0:
                _lookAtPlayer = false;
                break;
            case 1:
                _animator.SetInteger("step", _step);
                _lookAtPlayer = true;
                break;
            case 2:
                _animator.SetInteger("step", 0);
                break;
            case 3:
                break;
            case 4:
                StartCoroutine(TimeBeforeGameLose());
                break;
            default:
                break;
        }
        StartCoroutine(SetTimerForNextScareLevel());
    }

    private IEnumerator SetTimerForNextScareLevel()
    {
        yield return new WaitForSeconds(2);
        _canIncreaseNoiseLevel = true;
    }

    // ANIMATION CALLBACKS
    public void SetWaitTimeAndhide(float time)
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
        _stepTimer = time;
        _playingAnimation = false;
    }

    public void SetRandomWaitTime()
    {
        _stepTimer = Random.Range(1, 5);
        _playingAnimation = false;
    }

    public void CanEndGame()
    {
        GameController.Instance.canEndGame = true;
    }

    private IEnumerator TimeBeforeGameLose()
    {
        GameController.Instance.ChangeGameState(GameState.GAME_LOSE);
        yield return new WaitForSeconds(1.5f);
        GameController.Instance.ChangeUIState(UIState.GAME_END);
    }

    // PAUSE CALLBACKS
    public void HandlePause()
    {
        _animator.speed = 0;
    }

    public void HandleUnpause()
    {
        _animator.speed = 1;
    }
}
