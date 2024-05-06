using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeAnimation : MonoBehaviour
{
    public float rotationSpeed = 1000;

    [SerializeField] private bool turnOff = false;
    private bool off = false;

    void Update()
    {
        if (off || GameController.Instance.state == GameState.PAUSE)
        {
            return;
        }

        this.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (turnOff)
        {
            rotationSpeed = 0;
            gameObject.SetActive(false);
        }
    }

    public void TurnFanOff()
    {
        turnOff = true;
    }
}
