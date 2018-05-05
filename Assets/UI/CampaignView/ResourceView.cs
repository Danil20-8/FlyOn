using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.GameModels.Campaign;

public class ResourceView : ContainerItemView {

    [SerializeField]
    Image resImage;
    [SerializeField]
    Text resName;
    [SerializeField]
    Text resAmount;

    protected override void OnSetModel(ref object model)
    {
        ResourceAdder res = (ResourceAdder)model;
        resName.text = res.name;
        resAmount.text = res.value.amount.ToString();

        resImage.sprite = Sprite.Create(GetComponentInParent<CampaignViewBehaviour>().LoadResTexture(res.name), new Rect(Vector2.zero, new Vector2(64, 64)), Vector2.one * .5f);
    }
}
