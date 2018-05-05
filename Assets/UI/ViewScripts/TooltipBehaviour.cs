using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform), typeof(ContainerItemView))]
public class TooltipBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    RectTransform tooltip;

    new RectTransform transform;
    ContainerItemView container;

    void Start()
    {
        transform = GetComponent<RectTransform>();
        container = GetComponent<ContainerItemView>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != gameObject)
            if (hit.GetComponentInParent<TooltipBehaviour>() != this)
                return;

        float x = GetX(transform) + transform.rect.width;
        if (x + tooltip.sizeDelta.x > Screen.width)
            x = GetX(transform) - tooltip.rect.width;

        float y = GetY(transform);
        if (y - tooltip.rect.height < 0)
            y = GetY(transform) + tooltip.rect.height;

        tooltip.position = new Vector2(x, y) + Vector2.Scale(tooltip.pivot, tooltip.rect.size) - new Vector2(0, tooltip.rect.height);

        var ct = tooltip.GetComponent<ContainerItemView>();
        if (ct != null)
            ct.SetModel(container.GetModel());

        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

    float GetX(RectTransform rt)
    {
        return rt.position.x - rt.pivot.x * rt.rect.width;
    }
    float GetY(RectTransform rt)
    {
        return rt.position.y - rt.pivot.y * rt.rect.height + rt.rect.height;
    }
}
