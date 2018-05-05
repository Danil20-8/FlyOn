using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MyLib;
using Assets.GameModels;
using MyLib.Modern;
public class SystemComponentView : ContainerNodeView {

    string k = "";
    public string key { get { return k; } set { _key = value; SetModel(new Tuple<string, SystemComponent>(k, model.Item2)); } }
    string _key { get { return k; } set { k = value.ToLower(); keyText.text = value.ToUpper();} }

    Tuple<string, SystemComponent> model { get { return (Tuple<string, SystemComponent>)base.GetModel(); } }
    [SerializeField]
    Text keyText;

    protected override void OnSetModel(ref object model)
    {
        if (model is Tuple<string, SystemComponent>)
        {
            var com = (Tuple<string, SystemComponent>)model;
            _key = com.Item1;
        }
        else
        {
            model = new Tuple<string, SystemComponent>("", (SystemComponent)model);
        }
    }
    new public Tuple<string, SystemComponent> GetModel()
    {
        return base.GetModel<Tuple<string, SystemComponent>>();
    }
    public string GetKey()
    {
        return model.Item1;
    }
    public SystemComponent GetItem()
    {
        return model.Item2;
    }
}
