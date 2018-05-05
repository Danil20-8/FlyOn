using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.GameModels;
using Assets.GameModels.Components;
using MyLib;
using MyLib.Modern;
using Assets.Other;
public class SystemDiagnoseableTrees : ContainerTreeView
{
    Dictionary<ContainerNodeView, SystemLink.Info> links;
    protected override void OnUpdateNodes()
    {
        var ls = forest.BuildForest(s => GetLink(s));
        foreach (var p in ls.trees.Cast<PowerLink>())
            p.Diagnose();

        links = forest.ByElements().Select(ls.ByElements(), (n, l) => new Tuple<ContainerNodeView, SystemLink.Info>(n, l.GetInfo()))
            .ToDictionary(t => t.Item1, t => t.Item2);
        foreach (var l in links)
            l.Key.line.color = new Color(1 - l.Value.lastEnergy, 0, l.Value.lastEnergy);
    }
    protected virtual SystemLink GetLink(ContainerNodeView node)
    {
        if (node is SystemComponentView)
            return SystemLink.Pack(((SystemComponentView)node).GetItem());
        else
            return SystemLink.Pack(new FakeComponent());
    }
    public SystemLink.Info GetInfo(SystemComponentView node)
    {
        return links[node];
    }
}

