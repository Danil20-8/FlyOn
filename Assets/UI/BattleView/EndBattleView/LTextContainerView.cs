using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Global;
[RequireComponent(typeof(Text))]
public class LTextContainerView : ContainerItemView {



    protected override void OnSetModel(ref object model)
    {
        GetComponent<Text>().text = Localization.GetGlobalString((string)model);
    }
}
