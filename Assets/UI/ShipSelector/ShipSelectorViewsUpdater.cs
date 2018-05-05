using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Global;
using Assets.GameModels;
using Assets.Other;

public class ShipSelectorViewsUpdater : SlideBehaviour {

    [SerializeField]
    ContainerListView ships;

    [SerializeField]
    Text shipName;

    [SerializeField]
    ShipView ship;

    ShipSystemModel model;

    void Start()
    {
        gameObject.AddComponent<PointerListenerBehaviour>()
            .AddClickListener<ContainerItemView>(SelectShip);
    }
    void SelectShip(MonoBehaviour behaviour)
    {
        var container = (ContainerItemView)behaviour;

        var shipName = container.GetModel<string>();

        this.shipName.text = shipName;

        model = GameResources.GetShipSystemModel(shipName);

        ship.LoadShip(model);
    }

    public ShipSelectorViewsUpdater()
    {
        label = "selector";
    }

    public override void OnSlide(params object[] args)
    {
        screenSlider.sharedData.Clear();

        ships.Clear();
        ships.AddRange(GameResources.GetShipsNames());
    }

    public override void OnOut()
    {
        ship.ClearShip();
        model = null;
        shipName.text = "";
    }

    public void CreateNew()
    {
        screenSlider.sharedData.Add("BuildMode", "new");
        screenSlider.MoveTo("HullSelector");
    }
    public void Edit()
    {
        if (model == null)
            return;

        screenSlider.sharedData.Add("BuildMode", "edit");
        screenSlider.sharedData.Add("shipName", shipName.text);
        screenSlider.sharedData.Add("ShipModel", model);
        screenSlider.MoveTo("builder");
    }
    public void Back()
    {
        screenSlider.MoveTo("MainMenu");
    }
}
