using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.Other;

public class DestroyObjectsGoal : IBattleGoal
{
    public readonly List<ShipController> goal;

    public DestroyObjectsGoal(IEnumerable<ShipController> goal)
    {
        this.goal = goal.ToList();
    }
    public bool Check()
    {
        Stack<ShipController> toRemove = new Stack<ShipController>();
        foreach (var s in goal)
            if (!s.alive)
                toRemove.Push(s);
        foreach (var s in toRemove)
            goal.Remove(s);
        return !goal.Any();
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
public class SurviveGoal : IBattleGoal
{
    float time;
    float timeStart;
    public SurviveGoal(float time)
    {
        this.time = time;
        timeStart = Time.time;
    }

    public bool Check()
    {
        return Time.time - timeStart > time;
    }

    public AIBehabiour GetAntiBehaviour()
    {
        throw new NotImplementedException();
    }

    public AIBehabiour GetBehaviour()
    {
        throw new NotImplementedException();
    }
}
