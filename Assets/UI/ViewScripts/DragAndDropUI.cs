using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using MyLib.Algoriphms;
public class DragAndDropUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    public Transform movingRect;
    Transform baseRect;
    [SerializeField]
    new Camera camera;
    Vector2 startDragPosition;
    Vector2 startPosition;
    Vector2 offset;

    void Start()
    {
        baseRect = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(movingRect);
        startPosition = transform.localPosition;
        startDragPosition = transform.localPosition;
        var rect = GetComponent<RectTransform>();
        offset = (Vector2)camera.WorldToScreenPoint(transform.position) - eventData.position - rect.rect.position;
    }

    public void OnMove(AxisEventData eventData)
    {
        var v = eventData.moveVector;
        transform.position = transform.position + new Vector3(v.x, v.y, 0);
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = startDragPosition + eventData.position - eventData.pressPosition - offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = startPosition;
        transform.SetParent(baseRect);
        var hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != null)
        {
            var bin = hit.GetComponent<BinBehaviour>();
            if (bin == null && hit.transform.parent != null)
                bin = hit.GetComponentInParent<BinBehaviour>();
            if (bin != null)
                if (bin.Put(GetComponent<ContainerItemView>()))
                    return;
        }
    }
}
