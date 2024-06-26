using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originalScale;
    private Vector3 scaledScale;
    private bool isScaled = false;

    void Start()
    {
        originalScale = transform.localScale;
        scaledScale = originalScale * 1.4f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = scaledScale;
        isScaled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        isScaled = false;
    }
}
