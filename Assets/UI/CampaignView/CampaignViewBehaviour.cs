using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModels.Campaign;
using Assets.Global;
using Assets.Other;
public class CampaignViewBehaviour : SlideBehaviour {

    [SerializeField]
    StageView stage;

    [SerializeField]
    ContainerListView resourceList;

    Campaign campaign;
    static Campaign.Stage nextStage;

    static IEnumerator move;

    static CampaignViewBehaviour currentView;

	void Start()
    {
        currentView = this;
        GetComponentInParent<MessageListener>().AddListener<Campaign.Stage>(MoveToStage);

        Restart();

        GameManager.instance.PinCurrent();
    }



    void MoveToStage(object sender, Campaign.Stage stage)
    {
        nextStage = stage;

        move.MoveNext();
    }

    IEnumerator Next()
    {
        while (true)
        {
            currentView.UpdateView();
            yield return null;
            BattleInfo info = null;
            if (nextStage.battle)
            {
                GameState.sharedData["BattleConfig"] = nextStage.battleConfig;
                GameState.sharedData["CampaignRunMode"] = CampaignRunMode.Continue;
                GameState.sharedData["CachedCampaign"] = campaign;
                GameManager.instance.GoTo("RandomBattle");
                yield return null;
                info = (BattleInfo)GameState.sharedData["BattleInfo"];
                if (info.winner == null)
                    continue;
            }

            nextStage.Transit(info, campaign);
        }
    }

    void UpdateView()
    {
        stage.SetModel(campaign.current);
        resourceList.Clear();
        resourceList.AddRange(campaign.resources);
    }

    public Texture2D LoadResTexture(string resName)
    {
        byte[] image = GameResources.Load(@"Campaign\" + campaign.name + @"\" + resName + ".png");
        if (image == null)
            image = GameResources.Load(@"Campaign\Default\DefaultImage.png");

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(image);
        tex.Apply();
        return tex;
    }

    [GameConsoleCommand]
    void Restart()
    {
#if DEBUG
        string campaignName = (string)GameState.sharedData["CampaignName", "TestCampaign"];
        CampaignRunMode mode = (CampaignRunMode)GameState.sharedData["CampaignRunMode", CampaignRunMode.New];
#else
        string campaignName = (string)GameState.sharedData["CampaignName"];
        CampaignRunMode mode = (CampaignRunMode)GameState.sharedData["CampaignRunMode"];
#endif

        switch (mode)
        {
            case CampaignRunMode.New:
                InitializeTestCampaign();
                //campaign = GameResources.Load<Campaign>(@"Campaign\" + campaignName + @"\" + campaignName + ".cpn");
                move = Next();
                break;
            case CampaignRunMode.Load:
                campaign = GameResources.Load<Campaign>(@"Campaign\Saves\" + campaignName + ".scpn");
                move = Next();
                break;
            default:
                campaign = (Campaign)GameState.sharedData["CachedCampaign"];
                break;
        }


        move.MoveNext();
    }

    void InitializeTestCampaign()
    {
        campaign = new Campaign("TestCampaign");
        campaign.kinds = new string[] { "Ships", "Fuel", "Metall" };
        Localization.instance.context = new string[] { "Campaign", campaign.name };
        Localization.instance.dontLocalize = false;

        var s = campaign.current;
        s.storyString = new LString("It was a deep time of tests");
        var n = new Campaign.Stage();
        n.actionString = new LString("Begin");
        n.storyString = new LString("And it is begun. And it has no way back.");

        n.battleConfig = new BattleConfig()
        {
            battleSize = new RangeValue<float>(0, 1000, 1000),
            shipsAmount = new RangeValue<int>(0, 10, 10),
            type = BattleType.DeathMatch,
            playerShip = new PlayerShip() { type = PlayerShip.ShipType.Random }
        };
        n.adder.Add(new ResourceAdder() { name = "Ships", value = 100 });
        n.adder.Add(new ResourceAdder() { name = "Fuel", value = 500 });

        n.adder.Add(new ResourceAdder() { name = "Ships", value = new ResourceValue(false, ResourceValueSource.MyDeadShipsAmount) });

        s.forwardStages.Add(n);
    }
}

public enum CampaignRunMode
{
    New,
    Load,
    Continue,
    Test
}
