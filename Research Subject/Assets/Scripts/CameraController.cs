using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float speed = 0.05f;

    private float _currSpeed = 0f;
    private float _cameraYRotation = 0;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;   
    }

    void Update()
    {
        _cameraYRotation += this._currSpeed;
        _cameraYRotation = Mathf.Clamp(_cameraYRotation, -60, 60);
        transform.localEulerAngles = Vector3.up * _cameraYRotation;
    }

    public void MoveCamera(HoverType hoverType) {
        int multiplier = 1;
        if (hoverType == HoverType.CAMERA_RIGHT) {
            multiplier = -1;
        }
        this._currSpeed = speed * multiplier;
    }

    public void StopMovingCamera(HoverType hoverType) {
        this._currSpeed = 0;
    } 
}
