using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class LastUpdateShipUpdater : IBattleUpdater, IAddShipAble, IDestroyShipAble
{
    List<ShipController> ships = new List<ShipController>();

    public void AddShip(ShipController ship)
    {
        ships.Add(ship);
    }

    public void DestroyShip(ShipController ship)
    {
        ships.Remove(ship);
    }

    public void Update()
    {
        int count = ships.Count;
        for (int i = 0; i < count; i++)
            ships[i].LastUpdate();
    }
}

