using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnterSenderBehaviour : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        var listener = GetComponentInParent<PointerListenerBehaviour>();

        if (listener != null)
            listener.OnEnter(gameObject);
    }
}