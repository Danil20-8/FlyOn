using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using MyLib.Algoriphms;

public abstract class AIBehabiour
{
    protected float[] shipWeghts = new float[] { 1f, 1f, 1f, 1f, 1f };
    protected virtual float GetGoalWeight(AIDriver driver) { return 0f; }
    public float GetWeight(AIDriver driver) { return shipWeghts[driver.shipClass] * GetGoalWeight(driver); }

    public abstract bool GetMovePoint(AIDriver driver, out Vector3 result);
    public abstract void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult);
}

public class AIAgressorBehaviour : AIBehabiour
{
    public AIAgressorBehaviour()
    {
        shipWeghts = new float[] { 1, .9f, .6f, .6f, .9f };
    }

    public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
    {
        var ships = driver.enemiesAround;
        int count = ships.Count;
        if (count == 0)
            return;

        ShipController nearestEnemy = ships[0];
        float sqrDistance = (driver.ship.tempTransform.position - nearestEnemy.tempTransform.position).sqrMagnitude / (nearestEnemy.shipClass * nearestEnemy.shipClass);
        ShipController ship;
        int i = 1;
        float tempSqrDistance;
        for (; i < count; i++)
        {
            ship = ships[i];
            if (ship.team.team != driver.shipTeam.team)
            {
                tempSqrDistance = (driver.ship.tempTransform.position - ship.tempTransform.position).sqrMagnitude / (ship.shipClass * ship.shipClass);
                if (tempSqrDistance < sqrDistance)
                {
                    nearestEnemy = ship;
                    sqrDistance = tempSqrDistance;
                }
            }
        }
        // adding only one enemy
        outEnemiesResult.Add(nearestEnemy);
    }

    public override bool GetMovePoint(AIDriver driver, out Vector3 result)
    {
        if (driver.enemy != null)
        {
            result = driver.enemy.tempTransform.position;
            return true;
        }
        else {
            result = Vector3.zero;
            return false;
        }
    }

    protected override float GetGoalWeight(AIDriver driver)
    {
        float r = 0;

        var ships = driver.enemiesAround;
        var count = ships.Count;

        int wShipClass = driver.shipClass * 2;

        if (count >= wShipClass)
            return 1f;
        int n = count - (count % 4);
        int i = 0;
        for (; i < n; i++)
        {
            r += ships[i].shipClass;
            r += ships[i++].shipClass;
            r += ships[i++].shipClass;
            r += ships[i++].shipClass;
            if (r >= wShipClass)
                return 1f;
        }
        for (; i < count; i++)
            r += ships[i].shipClass;

        r /= wShipClass;
        return r > 1f ? 1f : r; 
    }
}

public class AISelfPreservation : AIBehabiour
{
    public AISelfPreservation()
    {
        shipWeghts = new float[] { 1, 1, .6f, .6f, 1 };
    }

    public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
    {
        if (driver.ship.offenders.Count > 0)
        {
            outEnemiesResult.Add(driver.ship.offenders.WithMin(s => (s.ship.tempTransform.position - driver.shipPosition).sqrMagnitude).ship);
        }
    }

    public override bool GetMovePoint(AIDriver driver, out Vector3 result)
    {
        if (driver.ship.offenders.Count > 0)
        {
            Vector3 v = Vector3.zero;
            foreach (var o in driver.ship.offenders)
                v += (o.ship.tempTransform.position - driver.ship.tempTransform.position);
            result = driver.ship.tempTransform.position + Vector3.Cross(driver.ship.tempTransform.forward, v);
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
        float r = 0;

        var ships = driver.ship.offenders;
        var count = ships.Count;
        for (int i = 0; i < count; i++)
            r+= ships[i].ship.shipClass;


        r *= (1f - driver.ship.health * .5f) /(driver.shipClass * 2);
        return (r > 1f ? 1f : r);
    }
}