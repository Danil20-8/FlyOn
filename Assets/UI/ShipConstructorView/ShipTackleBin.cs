using UnityEngine;
using System.Collections;
using Assets.GameModels;
using Assets.GameModels.Components;

public class ShipTackleBin : BinBehaviour {

    ShipConstructorBehaviour constructor;

    void Start()
    {
        constructor = GameObject.FindObjectOfType<ShipConstructorBehaviour>();
    }

    public override bool Put(ContainerItemView item)
    {
        if (GetComponent<ContainerItemView>() != null)
            return false;
        if (item.labelItem == "free")
        {
            if (((ConstuctorItemView)item).placed)
                return false;
        }
        else if (item.labelItem == "installed")
        {
            item.transform.position = item.GetComponent<DADItem>().start;
            item.transform.parent.GetComponent<HullBehaviour>().SetTackle(item.transform, SystemLink.Pack(new FakeComponent()));
            constructor.RemoveTackle(item.transform);
        }
        else
            return false;
        var hull = transform.parent.GetComponent<HullBehaviour>();

        var c = hull.SetTackle(transform, item.GetModel<SystemLink>());

        var citem = c.gameObject.AddComponent<ContainerItemView>();
        citem.SetLabel("installed");
        citem.SetModel(item.GetModel());
        c.gameObject.AddComponent<DADItem>();
        c.gameObject.AddComponent<FreeDragBehaviour>().offset = new Vector3(2, 2, 0);

        if (!constructor.AddTackle(c.transform))
            return false;

        item.Release();
        return true;
    }
}
