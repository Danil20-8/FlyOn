using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ScrollView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{

    IScrollable view;
    [SerializeField]
    float sensetive = 1;
    bool selected = false;
    void Start()
    {
        view = GetComponent<IScrollable>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (selected)
        {
            view.Scroll(eventData.scrollDelta.y * sensetive);
        }
    }
}

public interface IScrollable
{
    void Scroll(float value);
}