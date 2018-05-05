using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.GameModels.Campaign;

public class StageView : ContainerItemView {

    [SerializeField]
    Text storyText;

    [SerializeField]
    ContainerListView choiseList;

    protected override void OnSetModel(ref object model)
    {
        Campaign.Stage stage = (Campaign.Stage)model;

        storyText.text = stage.storyString;

        choiseList.Clear();
        foreach (var s in stage.forwardStages)
            choiseList.AddItem(s);
    }
}
