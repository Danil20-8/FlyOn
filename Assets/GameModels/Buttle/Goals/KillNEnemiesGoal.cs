using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Global;

public class KillNEnemiesGoal : IBattleGoal
{
    ShipTeam enemyTeam;
    int need;

    public KillNEnemiesGoal(int need, ShipTeam enemyTeam)
    {
        this.enemyTeam = enemyTeam;
        this.need = need;
    }

    public bool Check()
    {
        return enemyTeam.casualties.amount >= need;
    }

    public AIBehabiour GetAntiBehaviour()
    {
        return null;
    }

    public AIBehabiour GetBehaviour()
    {
        return null;
    }
}