using UnityEngine;
using System.Collections;
using Assets.GameModels.Campaign;

public class StagePass : MonoBehaviour {

    [SerializeField]
    TableView table;


	public void Enter()
    {
        table.Clear();

        Campaign.Stage stage = new Campaign.Stage();


        table.SetModel<ValueOption>(
            vo => vo.optionName,
            vo => vo.value
            );

        table.AddRange(
            new ValueOption[]
            {
                new ValueOption() { optionName = "ActionString", value = stage.actionString.text, setAction = (v) => stage.actionString.text = (string)v },
                new ValueOption() { optionName = "StoryString", value = stage.storyString.text, setAction = (v) => stage.storyString.text = (string)v },

            }
            );
        
	}
}
