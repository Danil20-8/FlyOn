using UnityEngine;
using System.Collections;
using System.Linq;
using MyLib.Modern;
using Assets.GameModels;
public class InventoryBinBehavior : BinBehaviour {

    [SerializeField]
    ContainerListView inventory;
    public override bool Put(ContainerItemView item)
    {
        if (!(item is ContainerNodeView))
            return false;
        if (!((ContainerNodeView)item).ReleaseNode())
            return false;

        item.Release();
        return true;
    }
}
