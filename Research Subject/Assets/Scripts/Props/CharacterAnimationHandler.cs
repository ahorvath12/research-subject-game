using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum CharacterAnimStates
{
    DEFAULT,
    IDLE,
    WALKING,
    TALKING,
    LOOKING
}

[Serializable]
public class CharacterAnimData
{
    public CharacterAnimStates state;
    public Vector3 targetPos;
    public float targetYRot;
    public float timeBeforeNextAnim;
    public float movementSpeed;
}

public class CharacterAnimationHandler : MonoBehaviour
{
    [SerializeField]
    public List<CharacterAnimData> animData = new List<CharacterAnimData>();

    private int _animIndex = 0;
    private int _nextAnimIndex = 0;
    private float _timer = 0;
    private float _currentWaitTimer = 0;
    private float _moveTime = 0;

    private Vector3 _initialPos;

    private Animator _animator;

    void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();

        _currentWaitTimer = animData[0].timeBeforeNextAnim;
        _nextAnimIndex = 1;

        GameController.Instance.SubscribeToPause(PauseAction);
        GameController.Instance.SubscribeToUnpause(UnpauseAction);
    }

    void Update()
    {
        if (GameController.Instance.state != GameState.RUNNING)
        {
            return;
        }
        if (_animIndex >= animData.Count || _nextAnimIndex >= animData.Count)
        {
            this.gameObject.SetActive(false);
            return;
        }

        _timer = GameController.Instance.timer;
        if (_currentWaitTimer > 0)
        {
            _currentWaitTimer -= Time.deltaTime;
            return;
        }

        if (_nextAnimIndex != _animIndex)
        {
            _animIndex = _nextAnimIndex;
            HandleNextAnimation();
        }

        _moveTime += Time.deltaTime * animData[_animIndex].movementSpeed;
        Vector3 targetPos = animData[_animIndex].targetPos;
        this.gameObject.transform.position = Vector3.Lerp(_initialPos, targetPos, _moveTime);

        if (Vector3.Distance(this.gameObject.transform.position, targetPos) <= 0.1f)
        {
            this.gameObject.transform.position = targetPos;

            _nextAnimIndex++;
            _currentWaitTimer = animData[_animIndex].timeBeforeNextAnim;
        }
    }

    void HandleNextAnimation()
    {
        _initialPos = this.gameObject.transform.position;
        _animator.SetInteger("state", (int)animData[_animIndex].state);
        Vector3 currRotation = this.gameObject.transform.eulerAngles;
        this.gameObject.transform.eulerAngles = new Vector3(currRotation.x, animData[_animIndex].targetYRot, currRotation.z);
        _moveTime = 0;
    }

    void PauseAction()
    {
        _animator.playbackTime = 0;
    }

    void UnpauseAction()
    {
        _animator.playbackTime = 1;
    }
}
