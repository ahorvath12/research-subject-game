using UnityEngine;

public class GuiToWorldSpace : MonoBehaviour
{
    public Transform anchor;

    void Start()
    {
        this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, anchor.position);
    }
}
