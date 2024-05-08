using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public enum HoverType {
    DEFAULT,
    CAMERA_RIGHT,
    CAMERA_LEFT,
    CAMERA_BOTTOM,
    SURVEY_TOP,
    SURVEY_BUTTON,
}

[System.Serializable]
public class HoverEvent : UnityEvent<HoverType>{}

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool detectHoverOnAwake = true;
    public HoverType hoverType;
    public HoverEvent onHoverEnterEvent, onHoverExitEvent;

    private bool _hasExitedHoverAfterAwake = false;

    private float _timeAtStart;
    private float _bufferTime;
    private int _enableCount = 0;

    void OnEnable()
    {
        _timeAtStart = Time.time;
        if (_enableCount <= 1)
        {
            _bufferTime = 1;
        }
        else
        {
            _bufferTime = 0.01f;
        }
        _enableCount++;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!this.isActiveAndEnabled) {
            return;
        }

        if (!detectHoverOnAwake && Time.time - _timeAtStart < _bufferTime)
        {
            return;
        }

        onHoverEnterEvent.Invoke(hoverType);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!this.isActiveAndEnabled) {
            return;
        }

        onHoverExitEvent.Invoke(hoverType);
    }
}
