using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Joystick joystick; // Tham chiếu đến Joystick thật

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.ShowAt(eventData.position);
        joystick.OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        joystick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.OnPointerUp(eventData);
    }
}
