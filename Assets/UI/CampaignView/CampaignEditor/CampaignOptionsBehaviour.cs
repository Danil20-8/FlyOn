using UnityEngine;
using System.Collections;
using Assets.GameModels.Campaign;

public class CampaignOptionsBehaviour : MonoBehaviour {

    [SerializeField]
    TableView options;

	public void Start()
    {
        Campaign camp = new Campaign("");
        options.SetModel<ValueOption>(
            vo => vo.optionName,
            vo => vo
            );

        options.AddRange(
            new ValueOption[]
            {
                new ValueOption() { optionName = "Resources", value = new string[0], setAction = (v) => camp.kinds = (string[])v }
            }
            );
    }
}
