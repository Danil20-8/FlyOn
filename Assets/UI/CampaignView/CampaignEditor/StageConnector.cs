using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(CanvasDragBehaviour))]
public class StageConnector : CanvasLineBeahviour, IPointerUpHandler
{


    public EditorStageView origin { get; set; }

    public EditorStageView end { get; set; }

    public bool isMain { get; set; }

    void Start()
    {
        GetComponent<CanvasDragBehaviour>().onDragOverride = OnDrag;
    }

    void OnDrag(Vector3 position)
    {
        if (isMain)
        {
            Vector3 pivot = ((Vector2)position - originPos);
            pivot.x = pivot.x < 0 ? 10 : -10;
            pivot.y = pivot.y < 0 ? 10 : -10;
            base.SetLine(originPos, position + pivot);
        }
        else
        {
            Vector3 pivot = ((Vector2)position - endPos);
            pivot.x = pivot.x < 0 ? 10 : -10;
            pivot.y = pivot.y < 0 ? 10 : -10;
            base.SetLine(position + pivot, endPos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        base.SetLine(originPos, endPos);

        var origin = this.origin;
        var end = this.end;

        var hit = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<EditorStageView>();

        if (hit == end)
        {
            origin.Remove(end, true);
        }
        else if(hit != null)
        {
            if (isMain)
            {
                if (hit != origin)
                    if (origin.Remove(end))
                        hit.AddForward(end);
            }
            else
            {
                origin.Remove(end);
                origin.SetRef(hit);
            }
        }
    }

    Vector2 originPos;
    Vector2 endPos;
    new public void SetLine(Vector2 origin, Vector2 end)
    {
        originPos = origin;
        endPos = end;

        base.SetLine(origin, end);
    }
}
