using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class FreeDragBehaviour : DragBehaviour, IPointerDownHandler, IDragHandler{

    Plane plane;
    [SerializeField]
    Transform pivot;
    public Vector3 offset = Vector3.zero;
    void Start()
    {
        if(pivot != null)
            offset = pivot.position - transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        plane = new Plane(Camera.main.transform.forward, transform.position);
        OnPick();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(eventData.position);
        float t;
        plane.Raycast(ray, out t);
        OnDrag(ray.GetPoint(t) - offset);
    }
}
