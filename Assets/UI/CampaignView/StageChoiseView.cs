using UnityEngine;
using System.Collections;
using Assets.GameModels.Campaign;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class StageChoiseView : ContainerItemView, IPointerClickHandler {

    [SerializeField]
    Text choiseText;

    [SerializeField]
    Image battleIcon;

    protected override void OnSetModel(ref object model)
    {
        Campaign.Stage stage = (Campaign.Stage)model;

        choiseText.text = stage.actionString;

        if (!stage.battle)
            battleIcon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var ml = GetComponentInParent<MessageListener>();
        if(ml != null) ml.Send(this, GetModel());
    }
}
