using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public enum HoverType {
    CAMERA_RIGHT,
    CAMERA_LEFT,
    CAMERA_BOTTOM,
    SURVEY_TOP,
}

[System.Serializable]
public class HoverEvent : UnityEvent<HoverType>{}

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HoverType hoverType;
    public HoverEvent onHoverEnterEvent, onHoverExitEvent;
    

    public void OnPointerEnter(PointerEventData eventData) {
        if (!this.isActiveAndEnabled) {
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
