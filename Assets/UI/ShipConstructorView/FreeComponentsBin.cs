using UnityEngine;
using System.Collections;
using Assets.GameModels;
using Assets.GameModels.Components;
public class FreeComponentsBin : BinBehaviour {

    ShipConstructorBehaviour constructor;
    [SerializeField]
    ConstructorTree tree;

    void Start()
    {
        constructor = FindObjectOfType<ShipConstructorBehaviour>();
    }

    public override bool Put(ContainerItemView item)
    {
        if (item.labelItem != "installed")
            return false;

        constructor.RemoveTackle(item.transform);


        item.transform.parent.GetComponent<HullBehaviour>().SetTackle(item.transform, SystemLink.Pack(new FakeComponent()));

        tree.GetNode(item.GetModel()).Remove();
        item.Release();
        return true;
    }
}
