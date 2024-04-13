using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanAnimation : MonoBehaviour
{
    public int rotationSpeed = 1;

    void Update()
    {
        this.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
