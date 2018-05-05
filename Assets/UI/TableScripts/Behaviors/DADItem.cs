using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Other;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ContainerItemView))]
public class DADItem : MonoBehaviour, IPointerUpHandler {

    [SerializeField]
    GameObject entity;

    GameObject curr;
    public Vector3 start;

    DragBehaviour dragBehaviour;

    void Start()
    {
        dragBehaviour = GetComponent<DragBehaviour>();
        if (dragBehaviour == null)
            throw new Exception("DADItem object has no any DragBehaviour");

        dragBehaviour.onPickOverride = OnPick;
        dragBehaviour.onDragOverride = OnDrag;
    }

    void OnPick()
    {
        if (entity != null)
        {
            curr = (GameObject)Instantiate(entity, transform.position, transform.rotation, transform.root);
        }
        start = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(curr != null)
        {
            Destroy(curr);
            curr = null;
        }
        else
            transform.position = start;
        var hit = eventData.pointerCurrentRaycast.gameObject;
        if (hit != null)
        {
            var bin = hit.GetComponent<BinBehaviour>();
            if (bin == null && hit.transform.parent != null)
                bin = hit.GetComponentInParent<BinBehaviour>();
            if (bin != null)
                if(bin.gameObject != gameObject)
                    if (bin.Put(GetComponent<ContainerItemView>()))
                        return;
        }
    }
    void OnDrag(Vector3 position)
    {
        if (curr != null)
            curr.transform.position = position;
        else
            dragBehaviour.BaseOnDrag(position);
    }
}
