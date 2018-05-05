using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.GameModels;
using System;

public class ShipItemView : ContainerItemView{

    [SerializeField]
    Text text;

    protected override void OnSetModel(ref object model)
    {
        text.text = (string)model;
    }

}
