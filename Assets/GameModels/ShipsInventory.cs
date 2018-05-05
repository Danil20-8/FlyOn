using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;
using MyLib.Serialization;

namespace Assets.GameModels
{
    public class ShipsInventory
    {
        [Addon]
        List<ShipSystemModel> ships = new List<ShipSystemModel>();
        public IEnumerable<ShipSystemModel> Ships { get { return ships; } }

        public void AddShip(ShipSystemModel ship)
        {
            ships.Add(ship);
        }
        public ShipSystemModel GetShip(string name)
        {
            var ship = ships.Where(s => s.name == name);
            return ship.Any() ? ship.First() : null;
        }

        public void Disassemble(string name, Inventory inventory)
        {
            var ship = ships.Where(s => s.name == name).FirstWN();
            if (ship == null)
                return;
            if (ships.Remove(ship))
                foreach (var c in ship.system.ByElements().Skip(1).Select(s => s.item))
                    inventory.AddItem(c);
        }

    }
}
