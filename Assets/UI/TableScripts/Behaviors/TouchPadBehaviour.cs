using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TouchPadBehaviour : MonoBehaviour, IDragHandler
{

    [SerializeField]
    Transform dragObject;

    void Start()
    {
        if (dragObject == null)
            dragObject = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragObject.position = dragObject.position + (Vector3)(eventData.delta);
    }

}
