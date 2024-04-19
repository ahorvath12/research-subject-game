using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanAnimation : MonoBehaviour
{
    public float rotationSpeed = 1000;
    public int decreaseSpeed = 500;

    private bool turnOff = false;
    private bool off = false;

    void Update()
    {
        if (off) {
            return;
        }

        this.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (turnOff) {
            rotationSpeed -= decreaseSpeed * Time.deltaTime;
            if (rotationSpeed <= 0) {
                off = true;
            }
        }
    }

    public void TurnFanOff() {
        turnOff = true;
    }
}
