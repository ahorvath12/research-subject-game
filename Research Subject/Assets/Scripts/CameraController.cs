using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float horizontalSpeed = 0.05f;
    public float verticalSpeed = 2f;
    public float lookDownXRot = 45f;

    private float _currSpeed = 0f;
    private float _cameraYRotation = 0f;

    private GameController _gameController;
    private UIState _state;
    private UIState _prevState;

    private Coroutine _activeCoroutine;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;   

        _gameController = GameController.Instance;
        _state = _gameController.uiState;

        GameController.Instance.SubscribeToPause(HandlePause);
    }

    void Update()
    {
        if (_state != _gameController.uiState) {
            _state = _gameController.uiState;

            switch(_state) {
                case UIState.START_VIEW_ROOM:
                    _activeCoroutine = StartCoroutine(LookUp());
                    break;
                case UIState.START_VIEW_SURVEY:
                    _activeCoroutine = StartCoroutine(LookDown());
                    break;
                default:
                    break;
            }

            return;
        }

        
        if (_state == UIState.VIEW_ROOM) {
            _cameraYRotation += _currSpeed;
            _cameraYRotation = Mathf.Clamp(_cameraYRotation, -30,30);
            transform.localEulerAngles = Vector3.up * _cameraYRotation;
        }
    }

    public void MoveCamera(HoverType hoverType) {
        int multiplier = 1;
        if (hoverType == HoverType.CAMERA_LEFT) {
            multiplier = -1;
        }
        _currSpeed = horizontalSpeed * multiplier;
    }

    public void StopMovingCamera(HoverType hoverType) {
        _currSpeed = 0;
    } 

    private IEnumerator LookDown() {
        while(transform.localEulerAngles.x < lookDownXRot) {
            transform.Rotate(Vector3.right, verticalSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.localEulerAngles = new Vector3(lookDownXRot,transform.localEulerAngles.y, transform.localEulerAngles.z);
        _gameController.uiState = UIState.VIEW_SURVEY;
        _activeCoroutine = null;
    }

    private IEnumerator LookUp() {
        while(transform.localEulerAngles.x > 0) {
            transform.Rotate(Vector3.right, -verticalSpeed * Time.deltaTime);

            // for when rotation wraps around to 360
            if (transform.localEulerAngles.x > lookDownXRot) { 
                transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y, transform.localEulerAngles.z);
            }

            yield return new WaitForEndOfFrame();
        }
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        _gameController.uiState = UIState.VIEW_ROOM;
        _activeCoroutine = null;
    }

    private void HandlePause()
    {
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
        }

        _prevState = _state;
    }

    private void HandleUnpause()
    {
        _state = _prevState;
    }
}
