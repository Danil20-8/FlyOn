using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class Widget : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerClickHandler
{
    public bool locked { get; set; }
    Vector2 startPosition;
    float doubleClickTime = 0;
    void Awake()
    {
        locked = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = transform.localPosition;
    }

    public void OnMove(AxisEventData eventData)
    {
        var v = eventData.moveVector;
        transform.position = transform.position + new Vector3(v.x, v.y, 0);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (locked)
            return;
        transform.localPosition = startPosition + eventData.position - eventData.pressPosition;
        var t = GetComponent<RectTransform>();
        var srect = t.rect;
        var prect = transform.parent.GetComponent<RectTransform>().rect;
        
        if (t.localPosition.x < prect.xMin) t.localPosition = new Vector3(prect.xMin, t.localPosition.y);
        if (t.localPosition.y < prect.yMin) t.localPosition = new Vector3(srect.xMin, t.localPosition.y);
        if (t.localPosition.x + srect.xMax > prect.xMax) t.localPosition = new Vector3(t.localPosition.x + (prect.xMax - (t.localPosition.x + srect.xMax)), t.localPosition.y);
        if (t.localPosition.y + srect.yMax > prect.yMax) t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y + (prect.yMax - (t.localPosition.y + srect.yMax)));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var time = Time.time;
        if (time - doubleClickTime < .3f)
            locked = !locked;
        else
            doubleClickTime = time;
    }
}
