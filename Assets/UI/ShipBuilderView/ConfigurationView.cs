using UnityEngine;
using System.Collections.Generic;
using Assets.GameModels;
using System.Linq;
using Assets.Other;
using MyLib.Modern;
public class ConfigurationView : ContainerItemView {

    [SerializeField]
    ColorIndicatorView energyIndicator;

    [SerializeField]
    ContainerListView list;

    void Start()
    {
        GetComponentInParent<PointerListenerBehaviour>().AddEnterListener<SystemComponentView>((b) => EnterListener((SystemComponentView)b));
    }

    void EnterListener(SystemComponentView view)
    {
        var info = ((SystemDiagnoseableTrees)view.GetTree()).GetInfo(view);

        energyIndicator.SetValue(info.lastEnergy);
    }

    protected override void OnSetModel(ref object model)
    {
        list.Clear();

        SystemComponent sc;
        if (model is Tuple<string, SystemComponent>)
        {
            sc = ((Tuple<string, SystemComponent>)model).Item2;

            energyIndicator.gameObject.SetActive(true);
        }
        else {
            sc = (SystemComponent)model;
            energyIndicator.gameObject.SetActive(false);
        }
        


        foreach (var i in sc.GetInfo())
            list.AddItem(null, f => ((FieldView)f).SetField(i.Name, i.Value));


        var config = sc.GetConfig();
        foreach (var c in config.GetNames())
            list.AddItem(null, f => ((FieldView)f).SetField(c, config.GetField(c).ToString()));
    }
}
