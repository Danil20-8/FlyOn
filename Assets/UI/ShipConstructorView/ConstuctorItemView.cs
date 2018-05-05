using UnityEngine;
using System.Collections;
using Assets.GameModels;
using Assets.Global;
using UnityEngine.UI;
using System;

public class ConstuctorItemView : ContainerNodeView {

    [SerializeField]
    Text textKey;

    [SerializeField]
    GameObject tick;

    void Start()
    {
        tick.SetActive(false);
    }

    public bool placed { get; private set; }
    protected override void OnSetModel(ref object model)
    {
        if (!(model is SystemLink))
            return;
        placed = false;
        textKey.text = ((SystemLink)model).item.lName;
    }
    public override void Release(Action<GameObject> action = null)
    {
        placed = true;
        tick.SetActive(true);
    }
    public void Remove()
    {
        placed = false;
        tick.SetActive(false);
    }
}
