using UnityEngine;
using System.Collections.Generic;
using Assets.GameModels.Phisical;
using Assets.GameModels;
using Assets.Global;
using System.Linq;
using MyLib;
public class HullBehaviour : MonoBehaviour {

    TacklePlaceBehaviour[] tackles;
    public int tacklesCount { get { return tackles.Length; } }
    ShipController ship;

    public int shipClass;
    public void SetHull(HullModel model, Forest<SystemLink> link, ShipDriver driver)
    {
        var t = model.GetTackles(link);
        foreach (var e in t)
        {
            SetTackle(e.Key, e.Value)
                .SetDriver(driver);
        }
        foreach (var l in link.ByElements())
            l.enabled = true;
    }

    public ShipComponentBehaviour SetTackle(int place, SystemLink link)
    {
        ShipComponentBehaviour tackle = ShipComponentBehaviour.InstantiateBehaviour(link, tackles[place]);
        tackle.transform.SetParent(transform);
        Destroy(tackles[place].gameObject);
        tackles[place] = tackle.GetComponent<TacklePlaceBehaviour>();
        return tackle;
    }
    public ShipComponentBehaviour SetTackle(Transform place, SystemLink link)
    {
        return SetTackle(GetTacklePosition(place), link);
    }
    public int GetTacklePosition(Transform tackle)
    {
        for (int i = 0; i < tackles.Length; i++)
            if (tackles[i].transform == tackle) return i;
        throw new System.Exception("Tackle's not found");
    }
    public ShipController SetShip(ShipSystemModel model, ShipDriver driver)
    {
        this.ship = gameObject.AddComponent<ShipController>();
        this.ship.SetShip(shipClass, model, driver);
        SetHull(model.hull, this.ship.shipSystem.system, driver);
        this.ship.SetComponents(tackles.Select(t => t.GetComponent<ShipComponentBehaviour>()).Where(c => c != null).ToArray());
        return this.ship;
    }

    public int GetTackleCount()
    {
        return GetComponentsInChildren<TacklePlaceBehaviour>().Length;
    }
    protected void Awake()
    {
        tackles = GetComponentsInChildren<TacklePlaceBehaviour>();
    }
}
