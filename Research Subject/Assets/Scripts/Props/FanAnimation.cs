using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanAnimation : MonoBehaviour
{
    public float rotationSpeed = 1000;
    public int decreaseSpeed = 500;

    [SerializeField]private bool turnOff = false;
    private bool off = false;

    private AudioSource audioSource;

    void Start() {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (off || GameController.Instance.state == GameState.PAUSE) {
            return;
        }

        this.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (turnOff) {
            rotationSpeed -= decreaseSpeed * Time.deltaTime;
            audioSource.pitch -= 0.5f * Time.deltaTime;
            if (rotationSpeed <= 0) {
                off = true;
                audioSource.pitch = 0;
            }
        }
    }

    public void TurnFanOff() {
        turnOff = true;
    }
}
