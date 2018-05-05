using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using Assets.Other;
using MyLib.Modern;
using MyLib.Algoriphms;
using Assets.GameScripts.Battle;

namespace Assets.Global
{
    public class ShipTeam
    {
        public readonly int team;
        readonly List<IBattleGoal> _goals;
        public IBattleGoal[] goals { get { return _goals.ToArray(); } }
        public AIBehabiour[] behaviours { get { return (AIBehabiour[]) _behaviours.Clone(); } }
        public bool myTeam = false;
        AIBehabiour[] _behaviours;
        Action<int> goalsReached;
        int lockKey = 0;

        public int[] shipsLost = new int[5];
        public Casualty casualties;
        List<ShipDriver> _deadDrivers;

        public void UseDeadDrivers(Action<List<ShipDriver>> action)
        {
            action(_deadDrivers);
            _deadDrivers.Clear();
        }

        public void AddToDead(ShipDriver driver, ShipController killer)
        {
            shipsLost[driver.shipClass]++;
            casualties.Add(killer);
            _deadDrivers.Add(driver);
        }

        public ShipTeam(int team, IEnumerable<IBattleGoal> goals, Action<int> goalsReached)
        {
            this.team = team;
            this._goals = goals.ToList();
            this.goalsReached = goalsReached;

            _deadDrivers = new List<ShipDriver>();
        }
        public void AddGoal(IBattleGoal goal)
        {
            if (lockKey != 0)
                throw new Exception("You can't add goal now.");
            _goals.Add(goal);
        }
        public void CheckGoals()
        {
            foreach (var g in _goals)
                if (!g.Check())
                    return;
            goalsReached(team);
        }

        public static void SetGoals(IEnumerable<ShipTeam> teams)
        {
            foreach(var team in teams)
            {
                team._behaviours = new AIBehabiour[] { new AIAgressorBehaviour(), new AISelfPreservation() }.Concat(teams.SelectMany(t => t.goals.Select(g => t == team ? g.GetBehaviour() : g.GetAntiBehaviour())).Where(b => b != null)).ToArray();
            }
        }

        public void Update()
        {
            //dirty = true;
        }
        void Lock(int oldKey, int newKey)
        {
            if (lockKey == oldKey)
                lockKey = newKey;
        }

        public override string ToString()
        {
            return "Team" + team.ToString();
        }
    }
    

    public class BattleInfo
    {
        public readonly ShipTeam[] teams;

        public int shipsDied { get { return teams.Select(t => GetDeadShipsAmount(t)).Sum(); } }

        public int team1DeadShipsAmount { get { return GetDeadShipsAmount(teams[0]); } }
        public int team2DeadShipsAmount { get { return GetDeadShipsAmount(teams[1]); } }

        public int deadShips1Amount { get { return teams.Select(t => t.shipsLost[1]).Sum(); } }
        public int deadShips2Amount { get { return teams.Select(t => t.shipsLost[2]).Sum(); } }
        public int deadShips3Amount { get { return teams.Select(t => t.shipsLost[3]).Sum(); } }

        public int destroyedBySun { get { return teams.Select(t => t.casualties.bySun).Sum(); } }
        public int destroyedByLight { get { return teams.Select(t => t.casualties.byLight).Sum(); } }
        public int destroyedByAverage { get { return teams.Select(t => t.casualties.byAverage).Sum(); } }
        public int destroyedByHeavy { get { return teams.Select(t => t.casualties.byHeavy).Sum(); } }
        public int destroyedByStation { get { return teams.Select(t => t.casualties.byStation).Sum(); } }
        public int destroyedByPlayer { get { return teams.Select(t => t.casualties.byPlayer).Sum(); } }

        public ShipTeam winner { get { return teams.FirstOrDefault(t => t.goals.All(g => g.Check())); } }

        public int GetDeadShipsAmount(ShipTeam team)
        {
            return team.shipsLost.Sum();
        }

        public BattleInfo(IEnumerable<ShipTeam> teams)
        {
            this.teams = teams.ToArray();
        }

    }
}

public struct Casualty
{
    public int amount;

    public int bySun;
    public int byLight;
    public int byAverage;
    public int byHeavy;
    public int byStation;

    public int byPlayer;

    public void Add(ShipController killer)
    {
        if (killer == null)
            bySun++;
        else
        {
            switch (killer.shipClass)
            {
                case 1:
                    byLight++;
                    break;
                case 2:
                    byAverage++;
                    break;
                case 3:
                    byHeavy++;
                    break;
                case 4:
                    byStation++;
                    break;
            }

            if (killer.isPlayer)
                byPlayer++;
        }
        amount++;
    }

}