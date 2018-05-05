using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class MouseMoveHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    UnityEvent mouseEnter;

    [SerializeField]
    UnityEvent mouseExit;
    [SerializeField]
    float delay;
    float timeEvent;
    UnityEvent action;
    bool freeze = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        timeEvent = Time.time;
        action = mouseEnter;
        freeze = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        timeEvent = Time.time;
        action = mouseExit;
        freeze = false;
    }
    void Update()
    {
        if (!freeze)
            if (Time.time - timeEvent > delay)
            {
                freeze = true;
                if (action != null)
                    action.Invoke();
            }
    }
}
