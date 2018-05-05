using UnityEngine;
using System.Collections;
using Assets.Global;
using System.IO;
using System.Linq;
using MyLib.Algoriphms;
public class HullSelectorView : SlideBehaviour {

    [SerializeField]
    ContainerListView list;
    [SerializeField]
    ShipView hullPosition;
    string hullName = "";
    void Start()
    {
        label = "HullSelector";
        gameObject.AddComponent<PointerListenerBehaviour>()
            .AddClickListener<HullNameListItem>(b => SelectHull((HullNameListItem)b));
    }

    bool showFour = false;
    public override void OnSlide(params object[] args)
    {
        list.Clear();
        if(showFour)
            list.AddRange(GameResources.GetAllShiphulls().Select(h => h.name));
        else
            list.AddRange(GameResources.GetAllShiphulls().Where(h => h.shipClass != 4).Select(h => h.name));
    }
    [GameConsoleCommand]
    void ShowFour()
    {
        showFour = true;
        OnSlide();
    }

    public override void OnOut()
    {
        list.Clear();
        hullPosition.ClearShip();
    }
    void SelectHull(HullNameListItem hull)
    {
        hullName = hull.GetModel<string>();
        hullPosition.SetEmptyShip(hullName);
    }
    public void ToBuilder()
    {
        if (hullName == "")
            return;
        screenSlider.sharedData.Add("hullName", hullName);
        screenSlider.MoveTo("builder");
    }

    public void ToBuilderFree()
    {
        if (hullName == "")
            return;
        screenSlider.sharedData.Add("hullName", hullName);
        screenSlider.MoveTo("builder");
    }

    public void Back()
    {
        screenSlider.MoveTo("selector");
    }

}
