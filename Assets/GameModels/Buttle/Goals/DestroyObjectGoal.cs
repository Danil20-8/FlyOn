using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.Other;
using MyLib.Algoriphms;
public class DestroyObjectGoal : IBattleGoal
{
    public readonly ShipController goal;
    AIDestroyObjectGoal behaviour;
    AIAntiDestroyObjectGoal antiBehaviour;
    public DestroyObjectGoal(ShipController goal)
    {
        this.goal = goal;
        behaviour = new AIDestroyObjectGoal(this);
        antiBehaviour = new AIAntiDestroyObjectGoal(this);
    }
    public bool Check()
    {
        return !goal.alive;
    }

    public AIBehabiour GetAntiBehaviour()
    {
        return null;
        return antiBehaviour;
    }

    public AIBehabiour GetBehaviour()
    {
        return null;
        return behaviour;
    }
}
public class AIDestroyObjectGoal : AIBehabiour
{
    DestroyObjectGoal goal;
    public AIDestroyObjectGoal(DestroyObjectGoal goal)
    {
        shipWeghts = new float[] { 1, .5f, .5f, .5f, 1 };
        this.goal = goal;
    }

    public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
    {
        outEnemiesResult.Add(goal.goal);
    }

    public override bool GetMovePoint(AIDriver driver, out Vector3 result)
    {
        result = goal.goal.tempTransform.position - driver.shipPosition;
        return true;
    }

    protected override float GetGoalWeight(AIDriver driver)
    {
        return 1f;
    }
}
public class AIAntiDestroyObjectGoal : AIBehabiour
{
    DestroyObjectGoal goal;
    public AIAntiDestroyObjectGoal(DestroyObjectGoal goal)
    {
        shipWeghts = new float[] { 1, .5f, 1f, .75f, 1 };
        this.goal = goal;
    }

    public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
    {
        var offenders = goal.goal.offenders.Where(t => t.ship.team != driver.ship.team).Cache();
        if (offenders.Count > 0)
            outEnemiesResult.Add(offenders.WithMin(o => (o.ship.tempTransform.position - driver.shipPosition).sqrMagnitude).ship);
    }

    public override bool GetMovePoint(AIDriver driver, out Vector3 result)
    {
        var offenders = goal.goal.offenders.Where(s => s.ship.team != driver.ship.team).Cache();
        if (offenders.Count > 0)
        {
            result = offenders.WithMin(o => (o.ship.tempTransform.position - driver.shipPosition).sqrMagnitude).ship.tempTransform.position - driver.shipPosition;
            return true;
        }
        else
        {
            result = default(Vector3);
            return false;
        }
    }

    protected override float GetGoalWeight(AIDriver driver)
    {
        return goal.goal.offenders.Count > 0 ? 1f : 0f;
    }
}