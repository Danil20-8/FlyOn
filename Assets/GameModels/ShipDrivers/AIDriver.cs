using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.GameModels;
using MyLib.Algoriphms;
using Assets.Other;
using Assets.Other.Special;
using System.Linq;
using MyLib;
using Assets.Global;
public class AIDriver : ShipDriver {

    public ShipController enemy { get { return enemies[0]; } }
    FastList<ShipController> enemies;
    ShipSystem shipSystem;

    public Vector3 defaultMovePoint { get { return BattleBehaviour.current.battleCenter; } }

    AIBehabiour[] behaviours;
    float[] _behaviourWeights;

    public AIDriver(ShipTeam team) 
        : base(team)
    {
        BattleBehaviour.AddEvent(() => { behaviours = team.behaviours; _behaviourWeights = new float[behaviours.Length]; });

        enemies = new FastList<ShipController>(firePoints.maxPoints);
    }

    bool SeeEnemy()
    {
        if (enemy == null)
            return false;
        RaycastHit hit;
        //var seeHits = Physics.RaycastAll(ship.transform.position, enemy.transform.position - ship.transform.position);
        if (Physics.Raycast(ship.transform.position, enemy.transform.position - ship.transform.position, out hit))
        {
            //var hit = seeHits.WithMin(h => h.distance);
            return ShipIdentification.IsThisShip(hit.transform.root, enemy);
        }
        else
            return false;
    }
    public override void SlowUpdate()
    {
        base.SlowUpdate();

        bFire = SeeEnemy();

    }
    public override void FastUpdate()
    {
        base.FastUpdate();

        UpdateBehabiours();
        FindEnemies();
        GetFirePoints();

        Vector3 point = GetMovePoint();

        moveDirection = Quaternion.LookRotation(point - ship.tempTransform.position);
        bRotate = Quaternion.Dot(ship.tempTransform.rotation, moveDirection) < .99619f;
    }
    void UpdateBehabiours()
    {
        RecalcWeights();
        Algs.ShakerSort(behaviours, _behaviourWeights);
    }
    void RecalcWeights()
    {
        for (int i = 0; i < behaviours.Length; i++)
            _behaviourWeights[i] = -behaviours[i].GetWeight(this);
    }
    void FindEnemies()
    {
        enemies.Clear();
        enemies[0] = null;
        foreach (var b in behaviours)
        {
            int maxCount = firePoints.maxPoints - enemies.Count;
            if (maxCount == 0)
                break;
            b.GetEnemies(this, maxCount, enemies);
        }
    }
    void GetFirePoints()
    {
        firePoints.count = enemies.Count;
        for (int i = 0; i < enemies.Count; i++)
            firePoints.SetPoint(enemies[i].tempTransform.position, i);
    }
    Vector3 GetMovePoint()
    {
        foreach(var b in behaviours)
        {
            Vector3 point;
            if(b.GetMovePoint(this, out point))
                return BattleBehaviour.current.navigation.GetMovePoint(ship.tempTransform.position, point);
        }
        return BattleBehaviour.current.navigation.GetMovePoint(ship.tempTransform.position, defaultMovePoint);
    }
}
