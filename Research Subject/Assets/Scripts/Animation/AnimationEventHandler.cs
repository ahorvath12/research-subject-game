using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public GameObject objToEnable;
    public Animator nextAnimatorToEnable;

    private bool audioWasPlaying;

    private void Start()
    {
        GameController gameController = GameController.Instance;

        gameController.SubscribeToPause(PauseAction);
        gameController.SubscribeToUnpause(UnpauseAction);
    }

    public void ChangeGameState(GameState newState) {
        GameController.Instance.ChangeGameState(newState);
    }

    public void ChangeUIState(UIState newState) {
        GameController.Instance.ChangeUIState(newState);
    }

    public void ChangeMainMenuState(MainMenuState newState) {
        MainMenuGUI.Instance.Changestate(newState);
    }

    public void PlayNextAnimation() {        
        nextAnimatorToEnable.enabled = true;
    }

    public void PlayAudioClip(AudioClip clip) {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (!audioSource) {
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void EnableObj() {
        objToEnable.SetActive(true);
    }

    private void PauseAction()
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource && audioSource.isPlaying)
        {
            audioWasPlaying = true;
            audioSource.Pause();
        }

        Animator animator = this.gameObject.GetComponent<Animator>();
        animator.speed = 0;
    }

    private void UnpauseAction()
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource && audioWasPlaying)
        {
            audioWasPlaying = false;
            audioSource.Play();
        }

        Animator animator = this.gameObject.GetComponent<Animator>();
        animator.speed = 1;
    }
}
