using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    public RectTransform handle;
    public float handleRange = 100f;

    private RectTransform bg;
    private Canvas canvas;
    private Vector2 inputVector;
    private Vector2 startPosition;

    void Start()
    {
        bg = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPosition = bg.anchoredPosition;
        
    }

    public void ShowAt(Vector2 screenPosition)
    {
        bg.gameObject.SetActive(true);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out pos
        );

        bg.anchoredPosition = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bg,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        inputVector = pos / handleRange;
        if (inputVector.magnitude > 1)
            inputVector = inputVector.normalized;

        handle.anchoredPosition = inputVector * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        bg.anchoredPosition = startPosition;
        
    }

    public float Horizontal() => inputVector.x;
    public float Vertical() => inputVector.y;
}
