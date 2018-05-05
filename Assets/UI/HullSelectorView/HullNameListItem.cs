using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HullNameListItem : ContainerItemView {

    [SerializeField]
    Text text;

    protected override void OnSetModel(ref object model)
    {
        text.text = (string)model;
    }
}
