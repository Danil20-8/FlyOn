using UnityEngine;
using System.Collections.Generic;
using Assets.Global;
using Assets.GameModels;
using Assets.GameModels.Components;
using Assets.GameModels.Phisical;
using Assets.GameModels.ShipDrivers;
using System.Linq;
using Assets.Other;
using MyLib.Modern;
using MyLib;
using MyLib.Algoriphms;
using System;
using System.Collections;
using Assets.Other.Special;
using Assets.Other;
public class ShipView : MonoBehaviour {

    public HullBehaviour ship { get; private set; }
    
    float zoomScale { get { return GetComponentInParent<ScrollZoom>().SN(sz => sz.zoomScale, 0); } set { GetComponentInParent<ScrollZoom>().SN(sz => sz.zoomScale = value); } }
    float zoomSensetive { get { return GetComponentInParent<ScrollZoom>().SN(sz => sz.sensetive, 1); } set { GetComponentInParent<ScrollZoom>().SN(sz => sz.sensetive = value); } }


    public void SetEmptyShip(string hullName)
    {
        CreateHull(hullName);
        Forest<SystemLink> system = new Forest<SystemLink>();
        for (int i = 1; i <= ship.tacklesCount; i++)
            system.AddTree(new SystemLink(new FakeComponent()));
        HullModel hull = new HullModel(hullName, system.ByElements().Select((f, i) => new Tuple<int, int[]>(i, system.GetPath(f))).ToDictionary(t => t.Item1, t => t.Item2));
        SetShip(hull, system);
    }
    public void LoadShip(string shipName)
    {
        var model = GameResources.GetShipSystemModel(shipName);
        LoadShip(model);
    }
    public void LoadShip(ShipSystemModel model)
    {
        CreateHull(model.hull.hullName);
        SetShip(model.hull, new ShipSystem(model).system);
    }
    void SetShip(HullModel model, Forest<SystemLink> links)
    {
        ship.transform.SetParent(transform);
        ship.SetHull(model, links, new FakeDriver());

        float farPoint = ship.GetComponentsInChildren<MeshFilter>().Max(m => Vector3.Scale(m.transform.lossyScale, m.mesh.bounds.max).magnitude);
        ship.transform.localPosition = Vector3.forward * farPoint;

        zoomScale = farPoint / 2;
        SetRotatedTransform();
    }
    void CreateHull(string hullName)
    {
        ClearShip();
        ship = (HullBehaviour)Instantiate(GameResources.GetShipHull(hullName), transform.position, transform.rotation);
    }
    public void ClearShip()
    {
        if (ship != null)
            Destroy(ship.gameObject);
    }

    void SetRotatedTransform()
    {
        GetComponentInParent<ModelRotater>().SN(mr => mr.rotatedTransform = ship.transform);
    }
}
