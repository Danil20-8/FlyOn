using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.GameModels;
using Assets.Other;
using Assets.GameScripts.Battle;

public class RespawnBehaviour: IBattleUpdater
{
    RespawnData[] respawn;

    public float nextRespawn { get { return lastRespawn + time; } }

    float time;

    float lastRespawn = 0;


    public RespawnBehaviour(ShipTeam[] teams, Transform[] points, float[] distancies, float time)
    {
        respawn = new RespawnData[teams.Length];
        for(int i = 0; i < teams.Length; i++)
            respawn[i] = new RespawnData() { team = teams[i], point = points[i], distanceToCenter = distancies[i] };
        this.time = time;
    }

    public void Update()
    {
        float t = Time.time;
        if (t - lastRespawn >= time)
        {

            foreach (var r in respawn)
                r.Respawn();

            lastRespawn = t;
        }
    }

}

class RespawnData
{
    public ShipTeam team;
    public Transform point;
    public float distanceToCenter;

    CustomCollection<ShipController> shipContainer = new CustomCollection<ShipController> { add = s => BattleBehaviour.current.AddShip(s) };

    public void Respawn()
    {
        team.UseDeadDrivers(Respawn);
    }
    void Respawn(List<ShipDriver> drivers)
    {
        int count = drivers.Count;

        Vector3 forward = (BattleBehaviour.current.battleCenter - point.position).normalized;
        Vector3 right = Vector3.Cross(point.up, forward);

        Quaternion rotation = Quaternion.LookRotation(forward);
        Vector3 position = forward * -distanceToCenter;

        DistributionCube cube = new DistributionCube(count * 40, count * 20, count * 20, 4, 2, 2);

        BattleGenerator.GenerateShips(
            drivers,
            position,
            rotation,
            point.up,
            right,
            forward,
            cube,
            shipContainer
            );

    }
}

