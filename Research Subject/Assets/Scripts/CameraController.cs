using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    private float _cameraVerticalRotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        _cameraVerticalRotation -= inputY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90, 90);
        transform.localEulerAngles = Vector3.right * _cameraVerticalRotation;
    }
}
