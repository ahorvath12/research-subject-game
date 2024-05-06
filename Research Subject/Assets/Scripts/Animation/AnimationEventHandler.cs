using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public GameObject objToEnable;
    public Animator nextAnimatorToEnable;

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
}
