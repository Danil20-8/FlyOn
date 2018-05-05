using UnityEngine;
using System.Collections;
using Assets.GameModels;
using UnityEngine.UI;
public class InventoryItemView : StackContainerItemView {

    [SerializeField]
    Text nameText;

    protected override void OnSetModel(ref object model)
    {
        base.OnSetModel(ref model);
        if (!(GetModel() is SystemComponent))
            return;
        nameText.text = ((SystemComponent)GetModel()).lName;
    }

}
