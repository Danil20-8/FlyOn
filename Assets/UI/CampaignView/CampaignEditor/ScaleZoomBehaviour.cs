using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ScaleZoomBehaviour : MonoBehaviour, IScrollHandler
{
    [SerializeField]
    Transform zoomTransform;

    [SerializeField]
    float minZoom = .1f;
    [SerializeField]
    float maxZoom = 2f;
    [SerializeField]
    float sensetive = .1f;

    float current = 1f;

    public void OnScroll(PointerEventData eventData)
    {
        current = Mathf.Clamp(current + eventData.scrollDelta.y * sensetive, minZoom, maxZoom);



        zoomTransform.localScale = Vector3.one * current;
    }
}
