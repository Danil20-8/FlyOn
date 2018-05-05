using UnityEngine;
using System.Collections;

public class RemoveItemView : ContainerItemView {

    [SerializeField]
    ContainerItemView item;

    ContainerListView list;

    protected override void OnSetModel(ref object model)
    {
        item.SetModel(model);

        
    }


}
