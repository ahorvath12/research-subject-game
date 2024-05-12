using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject head;
    public Transform playerTarget;
    private Animator _animator;

    // animation vars
    private float _activationTime = 50;
    private int _step = 0;
    private float _stepTimer;
    private bool _lookAtPlayer = false;

    // noise vars
    private bool _timingNoise = false;
    private float _noiseTime = 0;
    private int _noiseLevel = 0; // goes up to 3
    private int _maxNoiseLevel = 3;

    private List<float> _noiseQueue = new List<float>();
    private int _noiseQueueMaxSize = 5;

    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GameController gameController = GameController.Instance;

        if (gameController.state != GameState.RUNNING || gameController.timer < gameController.monsterStartTime)
        {
            return;
        }

        if (gameController.timer >= gameController.maxTime - 5)
        {
            _animator.applyRootMotion = true;
            _animator.SetBool("end", true);
            _animator.SetInteger("step", 0);
            _animator.SetInteger("scare", 0);
        }

        HandleAnimation();

        Debug.Log(_noiseLevel);
    }

    void LateUpdate()
    {
        if (_step <= 4)
        {
            return;
        }

        HandleNoise();
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

        _step++;

        _animator.SetInteger("step", _step);
        _stepTimer = Random.Range(2, 5);
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
    }

    public void NoMoreNoiseAction()
    {
        _timingNoise = false;
        _noiseTime = 0;
    }

    private void HandleNoise()
    {
        // handle constant noise
        if (_timingNoise)
        {
            _noiseTime += Time.deltaTime;

            if (_noiseTime > 1 && _noiseLevel < _maxNoiseLevel)
            {
                _noiseLevel++;
                _noiseTime = 0;
            }
            else if (_noiseTime <= 1 && _noiseLevel > 0)
            {
                _noiseLevel--;
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

            if (averageNoiseBreakTime <= 1 && _noiseLevel < _maxNoiseLevel)
            {
                _noiseLevel++;
            }
            else if (averageNoiseBreakTime > 1 && _noiseLevel > 0)
            {
                _noiseLevel--;
            }
            _noiseQueue = new List<float> { _noiseQueue[_noiseQueueMaxSize - 1] }; // reset queue
        }
    }

    private void HandleNoiseLevel()
    {
        switch (_noiseLevel)
        {
            case 0:
                _lookAtPlayer = false;
                _animator.SetInteger("scare", _noiseLevel);
                break;
            case 1:
                _lookAtPlayer = true;
                break;
            case 2:
                _animator.SetInteger("scare", _noiseLevel);
                break;
            case 3:
                _animator.SetInteger("scare", _noiseLevel);
                break;
            default:
                break;
        }
    }

    public void SetWaitTime(float time)
    {
        _stepTimer = time;
    }
}
