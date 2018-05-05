using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.GameModels;

public class LinkView : ContainerItemView {

    [SerializeField]
    Image health;
    [SerializeField]
    Text name;

    SystemLink link;

    protected override void OnSetModel(ref object model)
    {
        link = (SystemLink)model;

        name.text = link.item.lName;
    }

    void Update()
    {
        health.color = Color.Lerp(Color.red, Color.green, link.health);
    }
}
