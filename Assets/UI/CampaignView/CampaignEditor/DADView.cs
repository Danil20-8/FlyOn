using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DADView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField]
    LinkType type;

    DragBehaviour dragBehaviour;

    Vector3 position;

    public void OnPointerDown(PointerEventData eventData)
    {
        position = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = position;
        switch (type)
        {
            case LinkType.New:
                FindObjectOfType<CampaignEditorBehaviourView>()
                    .CreateStage(eventData.position, GetComponentInParent<EditorStageView>());
                break;
            case LinkType.Ref:
                var stage = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<EditorStageView>();
                if (stage != null)
                    GetComponentInParent<EditorStageView>()
                        .SetRef(stage);
                break;

    }
    }

    public enum LinkType
    {
        New,
        Ref
    }
}
