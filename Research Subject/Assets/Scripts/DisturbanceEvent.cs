using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisturbanceType
{
    ANIMATION,
    LIGHT,
    SOUND,
    TV,
    FAN,
}

public class DisturbanceEvent : MonoBehaviour
{
    public DisturbanceType type;
    public bool triggerBeforeMonster;
    public int possibleSteps = 1;
    public bool mustOccur = false;


    private Animator animator;
    private List<float> timestamp = new List<float>();
    private bool doneRunning = false;

    private bool wasPlayingAudio = false;

    // for tv
    private List<bool> animsPlayed = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();

        for (int i = 0; i < possibleSteps; i++)
        {
            GameController.Instance.AddDisturbanceEvent(this);
        }

        if (type == DisturbanceType.FAN || type == DisturbanceType.LIGHT)
        {
            mustOccur = true;
        }
        if (type == DisturbanceType.TV)
        {
            for (int i = 0; i < possibleSteps; i++)
            {
                animsPlayed.Add(false);
            }
        }

        GameController.Instance.SubsribeToPause(HandlePause);
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

        if (!doneRunning)
        {
            for (int i = 0; i < timestamp.Count; i++)
            {
                if (timestamp[i] <= gameController.timer)
                {
                    HandleEvent();
                    timestamp.RemoveAt(i);
                    i--;

                    if (i < 0)
                    {
                        i = 0;
                    }
                }
            }
            if (timestamp.Count == 0)
            {
                doneRunning = true;
            }
        }
    }

    private void HandleEvent()
    {
        switch (type)
        {
            case DisturbanceType.ANIMATION:
                TriggerAnimation();
                break;
            case DisturbanceType.FAN:
                ShutDownFan();
                break;
            case DisturbanceType.LIGHT:
                LightFlicker();
                break;
            case DisturbanceType.SOUND:
                TriggerAudio();
                break;
            case DisturbanceType.TV:
                TriggerTv();
                break;
        }
    }

    private void HandlePause()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            wasPlayingAudio = audioSource.isPlaying;
            audioSource.Pause();
        }
        switch (type)
        {
            case DisturbanceType.ANIMATION:
                animator.speed = 0;
                break;
            case DisturbanceType.TV:
                animator.speed = 0;
                break;
        }
    }

    private void HandleUnpause()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource && wasPlayingAudio)
        {
            audioSource.Play();
        }

        switch (type)
        {
            case DisturbanceType.ANIMATION:
                animator.speed = 1;
                break;
            case DisturbanceType.TV:
                animator.speed = 1;
                break;
        }
    }

    private void TriggerAnimation()
    {
        animator.SetInteger("step", animator.GetInteger("step") + 1);
    }

    private void LightFlicker()
    {
        LightFlicker light = this.gameObject.GetComponent<LightFlicker>();
        light.TriggerLightFlicker();
    }

    private void TriggerAudio()
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
    }

    private void TriggerTv()
    {
        int randIndex = Random.Range(0, animsPlayed.Count);
        while (animsPlayed[randIndex])
        {
            randIndex = (randIndex + 1) % animsPlayed.Count;
        }
        animsPlayed[randIndex] = true;
        animator.SetBool("PlayAnim", true);
        animator.SetInteger("Val", randIndex);
    }

    public void TvEnd()
    {
        animator.SetBool("PlayAnim", false);
    }

    private void ShutDownFan()
    {
        FanAnimation fan = this.gameObject.GetComponent<FanAnimation>();
        fan.TurnFanOff();
    }

    public void SetTimestamp(float time)
    {
        timestamp.Add(time);
    }
}
