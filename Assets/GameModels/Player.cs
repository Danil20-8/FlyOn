using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Serialization;
namespace Assets.GameModels
{
    public class Player
    {
        [ConstructorArg(0)]
        public string name = "Player";
        [Addon]
        public string ship = "343";
        [Addon]
        public ShipsInventory ships { get; private set; }
        [Addon]
        public Inventory inventory { get; private set; }
        public ShipSystemModel currentShip { get { return ships.GetShip(ship); } }
        public Player(string name)
        {
            this.name = name;
            ships = new ShipsInventory();
            inventory = new Inventory();
        }
    }
}
