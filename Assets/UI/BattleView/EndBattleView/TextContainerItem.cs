using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextContainerItem : ContainerItemView {


    protected override void OnSetModel(ref object model)
    {
        GetComponent<Text>().text = (string)model;
    }
}
