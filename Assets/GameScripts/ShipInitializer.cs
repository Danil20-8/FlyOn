using UnityEngine;
using System.Collections;
using Assets.GameModels;
using Assets.Global;
public abstract class ShipInitializer : MonoBehaviour {

	protected void Start () {
        GetInitializer().Init(transform);
        Destroy(gameObject);
    }

    protected abstract ShipInitializerModel GetInitializer();
}

public abstract class ShipInitializerModel
{
    public ShipController Init(Transform transform)
    {
        return Init(transform.position, transform.rotation);
    }
    public ShipController Init(Vector3 position, Quaternion rotation)
    {
        var model = GetModel();
        var ship = (HullBehaviour)GameObject.Instantiate(GameResources.GetShipHull(model.hull.hullName), position, rotation);
        PreEnd(ship);
        var controller = ship.SetShip(model, GetDriver());
        End(ship);

        return controller;
    }
    protected abstract ShipSystemModel GetModel();
    protected abstract ShipDriver GetDriver();
    protected virtual void PreEnd(HullBehaviour hull)
    {

    }
    protected virtual void End(HullBehaviour hull)
    {
    }
}