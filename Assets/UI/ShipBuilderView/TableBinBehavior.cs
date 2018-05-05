using UnityEngine;
using System.Collections;
using Assets.GameModels;
public class TableBinBehavior : BinBehaviour {

    [SerializeField]
    ContainerTreeView shipSystem;

    public override bool Put(ContainerItemView item)
    {
        if (item is ContainerNodeView)
            return false;
        var pc = item.GetModel();
        if (!(pc is PowerComponent))
            return false;
        int count = shipSystem.Count();
        if (count >= (int)ScreenManager.currentManager.sharedData["count"])
            return false;
        FindObjectOfType<ShipBuilderViewsUpdater>().SetCurrentCount(count + 1);
        shipSystem.AddNode(pc);

        item.Release();
        return true;
    }
}
