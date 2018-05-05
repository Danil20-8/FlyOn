using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Global;
public class EndBattleViewBehaviour : SlideBehaviour {

    [SerializeField]
    Text whoWinText;

    [SerializeField]
    TableView shipList;

    void Start()
    {
        var info = BattleBehaviour.current.GetInfo();
        if (info.winner.myTeam)
        {
            whoWinText.text = Localization.GetGlobalString("Win");
            whoWinText.color = Color.blue;
        }
        else {
            whoWinText.text = Localization.GetGlobalString("Lose");
            whoWinText.color = Color.red;
        }

        var fields = info.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty)
            .Select(p => new EndBattleTableField() { name = p.Name, value = p.GetValue(info, null).ToString() }).ToArray();

        shipList.SetModel<EndBattleTableField>(
            s => s.name,
            s => s.value
            );

        shipList.AddRange(fields);
    }
}

class EndBattleTableField
{
    public string name;
    public string value;
}
