using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;

[Serializable]
public class BattleConfig
{
    public BattleType type = BattleType.DeathMatch;
    public RangeValue<int> shipsAmount = new RangeValue<int>(1, 50, 25);
    public RangeValue<float> battleSize = new RangeValue<float>(1000, 10000, 5000);
    public PlayerShip playerShip = new PlayerShip(PlayerShip.ShipType.Random, 1, "");
    public RangeValue<int> battleDurationMultipier = new RangeValue<int>(1, 10, 5);
    public RangeValue<float> lightShipsWeight = new RangeValue<float>(0, 1, 1);
    public RangeValue<float> averageShipsWeight = new RangeValue<float>(0, 1, 1);
    public RangeValue<float> heavyShipsWeight = new RangeValue<float>(0, 1, 1);
}

[Serializable]
public enum BattleType
{
    DeathMatch,
    BattleForTheStar,
    Escort
}

[Serializable]
public struct PlayerShip
{
    public ShipType type;

    public int shipClass;
    public string shipName;

    public PlayerShip(ShipType type, int shipClass, string shipName)
    {
        this.type = type;
        this.shipClass = shipClass;
        this.shipName = shipName;
    }

    public enum ShipType
    {
        Random,
        RandomOfClass,
        Concrete
    }
}
