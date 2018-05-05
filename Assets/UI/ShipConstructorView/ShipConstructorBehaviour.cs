using UnityEngine;
using System.Collections.Generic;
using Assets.GameModels;
using System.Linq;
using Assets.Other;
using Assets.GameModels.Phisical;
using Assets.Global;
using MyLib;
using MyLib.Algoriphms;
using Assets.GameModels.Components;
using Assets.Serialization;
public class ShipConstructorBehaviour : SlideBehaviour {

    Forest<SystemLink> links;
    Dictionary<string, int[]> keys;
    string hullName = "AverageHull";

    List<Transform> tackles = new List<Transform>();

    List<SystemLink> free = new List<SystemLink>();
    [SerializeField]
    ConstructorTree tree;
    [SerializeField]
    ShipView ship;

    [SerializeField]
    TextInputMessage shipNameInput;

    public ShipConstructorBehaviour()
    {
        label = "constructor";
    }

    public override void OnSlide(params object[] args)
    {
        links = (Forest<SystemLink>)screenSlider.sharedData["system"];
        keys = (Dictionary<string, int[]>)screenSlider.sharedData["keys"];
        hullName = (string)screenSlider.sharedData["hullName"];
        free = links.ByElements().ToList();

        tree.Clear();
        tree.SetTree(links.BuildForest(l => new TreeNode<object>(l)));
        ship.SetEmptyShip(hullName);
    }
    public override void OnOut()
    {
        ship.ClearShip();
        tackles.Clear();
        free.Clear();
    }
    public bool AddTackle(Transform tackle)
    {
        if (tackles.Has(tackle))
            return false;
        tackles.Add(tackle);
        return true;
    }
    public void RemoveTackle(Transform tackle)
    {
        tackles.Remove(tackle);
    }
    public void Build()
    {
        if (tackles.Count != free.Count)
            return;

        ScreenManager.ShowMessage(shipNameInput).SetMessageBox(Localization.GetGlobalString("Enter ship name"), BuildPlayerShip);
    }
    public void BuildFree()
    {
        if (tackles.Count != free.Count)
            return;

        ScreenManager.ShowMessage(shipNameInput).SetMessageBox(Localization.GetGlobalString("Enter ship name"), BuildFreeShip);
    }
    string BuildPlayerShip(string name)
    {
        if (name == "")
            return Localization.GetGlobalString("Name must have at least 1 symbol");
        foreach (var n in PlayerResources.ships.Ships)
            if (n.name == name)
                return Localization.GetGlobalString("Ship with same name alredy exists");

        PlayerResources.ships.AddShip(BuildShip(name));

        PlayerResources.inventory.DropItems(links.ByElements().Select(l => l.item).ToArray());
        screenSlider.MoveTo("selector");
        PlayerResources.Save();
        return "";
    }
    
    string BuildFreeShip(string name)
    {
        if (name == "")
            return Localization.GetGlobalString("Name must have at least 1 symbol");

        string result = "";
        GameResources.Save(@"Ships\" + name + ".txt", BuildShip(name),
            () => { result = Localization.GetGlobalString("Ship with same name alredy exists"); return false; });

        if (result != "")
            return result;

        screenSlider.MoveTo("selector");
        return result;
    }
    ShipSystemModel BuildShip(string name)
    {
        Dictionary<int, int[]> dt = new Dictionary<int, int[]>();
        foreach (var t in tackles)
        {
            var l = t.GetComponentInChildren<ContainerItemView>().GetModel<SystemLink>();
            dt.Add(ship.ship.GetTacklePosition(t), links.GetPath(l));
        }
        return new ShipSystemModel(name,
            links.BuildForest(n => new TreeNode<SystemComponent>(n.item)),
            keys,
            new HullModel(hullName, dt)
            );
    }

}

