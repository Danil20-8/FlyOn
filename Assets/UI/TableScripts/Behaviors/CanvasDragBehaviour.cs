using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class CanvasDragBehaviour : DragBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    Transform pivot;

    public void OnPointerDown(PointerEventData eventData)
    {

        OnPick();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pivot != null)
            OnDrag((Vector2)(transform.position - pivot.position) + eventData.position);
        else
            OnDrag(eventData.position);
    }
}
