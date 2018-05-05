using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointerEventHandlerBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField]
    UnityEvent downEvent = new UnityEvent();
    [SerializeField]
    UnityEvent upEvent = new UnityEvent();
    [SerializeField]
    UnityEvent clickEvent = new UnityEvent();
    [SerializeField]
    UnityEvent enterEvent = new UnityEvent();
    [SerializeField]
    UnityEvent exitEvent = new UnityEvent();


    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enterEvent.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exitEvent.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        upEvent.Invoke();
    }

    public void SetEvents(UnityAction clickEvent = null, UnityAction enterEvent = null, UnityAction exitEvent = null)
    {
        if(clickEvent != null)
            this.clickEvent.AddListener(clickEvent);
        if(enterEvent != null)
            this.enterEvent.AddListener(enterEvent);
        if(exitEvent != null)
            this.exitEvent.AddListener(exitEvent);
    }
}

