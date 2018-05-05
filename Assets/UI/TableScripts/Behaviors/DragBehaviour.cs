using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class DragBehaviour: MonoBehaviour
{
    public Action<Vector3> onDragOverride = null;
    public Action onPickOverride = null;

    public void OnPick()
    {
        if (onPickOverride != null)
            onPickOverride();
    }
    public void OnDrag(Vector3 position)
    {
        if (onDragOverride != null)
            onDragOverride(position);
        else
            BaseOnDrag(position);
    }
    public void BaseOnDrag(Vector3 position)
    {
        transform.position = position;
    }
}

