using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModels.Campaign;
using Assets.Global;
using Assets.UI.CampaignView.CampaignEditor;
using MyLib;
public class CampaignEditorBehaviourView : SlideBehaviour {

    [SerializeField]
    EditorStageView view;

    [SerializeField]
    Transform stageContainer;

    Campaign campaign = new Campaign("None adventure");

    // Use this for initialization
    void Start()
    {
        Localization.instance.context = new LocalizationContext("Campaign", campaign.name);
        Localization.instance.dontLocalize = true;

        var stage = (EditorStageView)Instantiate(view, stageContainer);
        stage.transform.localPosition = Vector3.zero;
        campaign.current.actionString = new LString("YourBegin");

        stage.SetModel(campaign.current);
        stages.Add(stage);

    }

    List<EditorStageView> stages = new List<EditorStageView>();

    public void CreateStage(Vector3 position, EditorStageView parent)
    {
        var stage = ((EditorStageView)Instantiate(view, position, parent.transform.rotation, stageContainer));
        stage.SetModel(new Campaign.Stage());
        parent.AddForward(stage);

        stages.Add(stage);
    }
    public void RemoveStage(EditorStageView stage)
    {
        stages.Remove(stage);
    }

    [GameConsoleCommand]
    void Save(string mode = "")
    {
        switch (mode)
        {
            case "release":
                GameResources.Save(@"Campaign\" + campaign.name + ".cpn", campaign, () => true);
                break;
            default:
                var meta = new CampaignMeta()
                {
                    campaign = campaign,
                    stages = stages.Select(s => s.GetModel<Campaign.Stage>().GetPath()).ToList(),
                    positions = stages.Select(s => s.transform.localPosition).ToArray()
                };

                GameResources.Save(@"Campaign\" + campaign.name + ".ecpn", meta, () => true);
                break;
        }
    }
    [GameConsoleCommand]
    void Load()
    {
        var meta = GameResources.Load<CampaignMeta>(@"Campaign\" + campaign.name + ".ecpn");

        foreach (var s in stages.ToArray())
            s.Destroy();

        campaign = meta.campaign;
        var rootStage = campaign.current.GetRoot();

        for (int i = 0; i < meta.stages.Count; i++)
        {
            var stage = Instantiate(view);
            stage.transform.SetParent(stageContainer, false);
            stage.transform.localPosition = meta.positions[i];

            stage.SetModel(rootStage.GetNode(meta.stages[i]));
            stages.Add(stage);
        }

        EditorStageView.ResetCouples(stages);
    }

    [GameConsoleCommand]
    void Test()
    {
        GameManager.instance.PinCurrent();

        GameManager.instance.GoTo("CampaignScene");
    }
}
