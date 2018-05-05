
using UnityEngine;
using System.Collections.Generic;
using Assets.GameModels;
using Assets.GameModels.Components;
using Assets.GameModels.Phisical;
using MyLib;
using MyLib.Modern;
using Assets.Global;
using System;

public class StationInitializerModel : ShipInitializerModel
{
    public StationInitializerModel(ShipTeam team)
    {
        this.team = team;
    }
    ShipTeam team;
    protected override ShipDriver GetDriver()
    {
        return new AIDriver(team);
    }

    protected override ShipSystemModel GetModel()
    {
        Forest<SystemLink> system = new Forest<SystemLink>();
        SystemLink power = SystemLink.Pack(new PowerComponent(4));
        SystemLink gun1 = SystemLink.Pack(new GunComponent(4));
        SystemLink gun2 = SystemLink.Pack(new GunComponent(4));
        power.AddNode(gun1);
        power.AddNode(gun2);
        system.AddTree(power);
        Dictionary<int, int[]> tackles = new Dictionary<int, int[]>()
        {
            {2, system.GetPath(power) },
            {0, system.GetPath(gun1) },
            {1, system.GetPath(gun2) },
        };
        return new ShipSystemModel("666", system.BuildForest(c => new TreeNode<SystemComponent>(c.item)), new Dictionary<string, int[]>(), new HullModel("Station", tackles));
    }
}