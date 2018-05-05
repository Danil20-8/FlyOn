using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ModelRotater : MonoBehaviour, IDragHandler {

    [SerializeField]
    float sensetive = 1;

    public Transform rotatedTransform;

    protected void Awake()
    {
        if (rotatedTransform == null)
            rotatedTransform = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rotatedTransform.eulerAngles += new Vector3(sensetive * -eventData.delta.y, sensetive * -eventData.delta.x, 0);
    }
}
