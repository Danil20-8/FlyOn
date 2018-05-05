using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndBattleShipViewBehaviour : ContainerItemView {

    [SerializeField]
    Text driverName;

    [SerializeField]
    Text shipName;

    [SerializeField]
    Text shipClass;

    protected override void OnSetModel(ref object model)
    {
        var ship = (ShipController)model;

        driverName.text = ship.name;

        shipName.text = ship.name;

        shipClass.text = ship.shipClass.ToString();

    }
}
