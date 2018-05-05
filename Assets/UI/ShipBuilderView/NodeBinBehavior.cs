using UnityEngine;
using System.Collections;
using Assets.Other;
using MyLib;
using Assets.GameModels;
using MyLib.Modern;
public class NodeBinBehavior : BinBehaviour {

    public override bool Put(ContainerItemView item)
    {
        ContainerNodeView root = GetComponent<ContainerNodeView>();
        if (item is ContainerNodeView)
        {
            if (((Tuple < string, SystemComponent > )item.GetModel()).Item2 is PowerComponent)
                return false;
            root.AddNode<ContainerNodeView>((ContainerNodeView)item);
        }
        else {
            if (item.GetModel() is PowerComponent)
                return false;
            int count = root.GetTree().Count();
            if (count >= (int)ScreenManager.currentManager.sharedData["count"])
                return false;
            FindObjectOfType<ShipBuilderViewsUpdater>().SetCurrentCount(count + 1);
            root.AddNode(item.GetModel());

            item.Release();
        }
        return true;
    }
}
