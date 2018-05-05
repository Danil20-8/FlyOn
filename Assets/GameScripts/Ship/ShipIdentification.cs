using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShipIdentification: MonoBehaviour
{
    public ShipController ship { get { return _ship; } set { if (_ship != null) throw new Exception("You can't change component holder"); _ship = value; _shipTransform = _ship.transform; } }
    public Transform shipTransform { get { return _shipTransform; } }
    ShipController _ship;
    Transform _shipTransform;

    public static bool IsThisShip(Transform shipIdentification, Transform shipTransform)
    {
        var si = shipIdentification.GetComponentInParent<ShipIdentification>();
        return si == null ? false : si.transform == shipTransform;
    }
    public static bool IsThisShip(Transform shipIdentification, ShipIdentification ship)
    {
        var si = shipIdentification.GetComponentInParent<ShipIdentification>();
        return si == null ? false : si._ship == ship._ship;
    }
    public static bool IsThisShip(Transform shipIdentification, ShipController ship)
    {
        var si = shipIdentification.GetComponentInParent<ShipIdentification>();
        return si == null ? false : si._ship == ship;
    }
}
