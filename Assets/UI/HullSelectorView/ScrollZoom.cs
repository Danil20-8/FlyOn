using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using Assets.Other;

public class ScrollZoom : MonoBehaviour, IScrollHandler {

    [SerializeField]
    Transform zoomObject;
    [SerializeField]
    Transform basePosition;
    [SerializeField]
    Vector3 axis = Vector3.forward;
    [SerializeField]
    LineBounds bounds = new LineBounds(0, 5);

    public float sensetive;

    public float zoomScale { get { return axis.magnitude; } set { axis = axis.normalized * value; } }

    float current = 0;

    protected void Awake()
    {
        if (bounds.left > 0 || bounds.right < 0)
            throw new Exception("Invalid bounds");
    }

    public void OnScroll(PointerEventData eventData)
    {
        current -= eventData.scrollDelta.y * sensetive;
        bounds.ToBounds(ref current);

        if (current > 0)
            zoomObject.transform.position = Vector3.Lerp(basePosition.transform.position, basePosition.transform.position + axis * bounds.right, current / bounds.right);
        else if (current < 0)
            zoomObject.transform.position = Vector3.Lerp(basePosition.transform.position, basePosition.transform.position + axis * bounds.left, current / bounds.left);
        else
            zoomObject.transform.position = basePosition.position;
    }
}
