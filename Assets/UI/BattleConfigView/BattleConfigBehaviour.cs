using System;
using UnityEngine;
using System.Collections;
using Assets.Global;
using Assets.Other;
using System.Reflection;
using System.Linq;

public class BattleConfigBehaviour : SlideBehaviour {

    BattleConfig config;

    [SerializeField]
    TableView table;


    void Start()
    {
        //Get last or initialize new config
        config = (BattleConfig)GameState.sharedData["BattleConfig", new BattleConfig()];

        table.SetModel<ValueOption>(
            vo => vo.optionName,
            vo => vo);

        table.AddRange(config.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Select(f => new ValueOption()
        {
            optionName = f.Name,
            value = f.GetValue(config),
            setAction = v => f.SetValue(config, v)
        }
        ).ToArray());

        GetComponent<ThemeListener>().UpdateTheme();
    }

    public void SetOptions(Action<BattleConfig> setAction)
    {
        setAction(config);
    }

    public void ToBattle()
    {
        GameManager.instance.GoTo("RandomBattle");
    }

    public void ToMainMenu()
    {
        screenSlider.MoveTo("MainMenu");
    }
}
