using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSenderBehaviour : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var listener = GetComponentInParent<PointerListenerBehaviour>();

        if (listener != null)
            listener.OnClick(gameObject);
    }
}

