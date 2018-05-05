using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Assets.Global;
using Assets.GameModels;
using Assets.GameModels.Phisical;
using System.Linq;
using Assets.GameModels.Components;
using MyLib;
using MyLib.Modern;
using Assets.Other.Special;
public class ShipBuilderViewsUpdater : SlideBehaviour {

    [SerializeField]
    ContainerListView inventory;


    [SerializeField]
    ContainerTreeView systemTree;

    [SerializeField]
    Text countText;

    void Awake()
    {
        gameObject.AddComponent<PointerListenerBehaviour>();
    }

    public ShipBuilderViewsUpdater()
    {
        label = "builder";
    }

    public override void OnSlide(params object[] args)
    {
        inventory.Clear();
        string buildMode = (string)screenSlider.sharedData["BuildMode"];
        switch (buildMode)
        {
            case "edit":
                string shipName = (string)screenSlider.sharedData["shipName"];
                if (shipName != "")
                {
                    var model = (ShipSystemModel)screenSlider.sharedData["ShipModel"];
                    var kc = model.system.BuildForest(n => new TreeNode<Tuple<string, SystemComponent>>(new Tuple<string, SystemComponent>("", n.item)));
                    model.GetKeys(kc, (n, k) => n.item.Item1 = k);

                    systemTree.SetTree(kc.BuildForest(n => new TreeNode<object>(n.item)));

                    screenSlider.sharedData.Add("hullName", model.hull.hullName);
                }
                else
                    throw new System.Exception("shipName's null. Please put shipName arg to the sharedData if you want edit ship.");
                break;
            case "new":
                break;
            case "free":
                break;
        }

        var hull = GameResources.GetShipHull((string)screenSlider.sharedData["hullName"]);
        inventory.AddRange(Enumerable.Range(0, 5).SelectMany(i => Inventory.GetExamples(i)).Concat(Inventory.GetBaseExamples(hull.shipClass)).Select(c => new InfiniteStack<SystemComponent>(c)));

        var count = hull.GetTackleCount();
        screenSlider.sharedData.Add("count", count);
        countText.text = "0 / " + count.ToString();
    }
    public void SetCurrentCount(int count)
    {
        countText.text = count.ToString() + " / " + (int) screenSlider.sharedData["count"];
    }
    public override void OnOut()
    {
        inventory.Clear();
        systemTree.Clear();
    }
    public void Build()
    {
        var tree = systemTree.GetTree(n => new TreeNode<Tuple<string, SystemComponent>>((Tuple<string, SystemComponent>)n));
        var system = tree.BuildForest(m => SystemLink.Pack(m.item.Item2));
        var keys = tree.ByElements().Where(n => n.item.Item1 != "").ToDictionary(n => n.item.Item1, n => tree.GetPath(n));
        screenSlider.sharedData.Add("system", system);
        screenSlider.sharedData.Add("keys", keys);
        screenSlider.MoveTo("constructor");
    }

}
