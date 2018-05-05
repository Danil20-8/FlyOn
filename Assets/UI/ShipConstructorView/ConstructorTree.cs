using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.GameModels;
using MyLib;
using MyLib.Modern;
using Assets.Other;
using Assets.GameModels.Components;
public class ConstructorTree: SystemDiagnoseableTrees
{
    protected override SystemLink GetLink(ContainerNodeView node)
    {
        var l = node.GetModel();
        if (l == null)
            return SystemLink.Pack(new FakeComponent());
        else
            return SystemLink.Pack(((SystemLink)l).item);
    }
    new public ConstuctorItemView GetNode(object link)
    {
        return (ConstuctorItemView) base.GetNode(link);
    }
}

