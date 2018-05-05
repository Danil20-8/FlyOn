using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using MyLib.Algoriphms;
using System.Collections;
using Assets.GameScripts.Battle;

namespace Assets.GameScripts.Battle
{
    public class Navigation
    {
        List<Obstacle> obstacles = new List<Obstacle>();

        public void AddObstacles(IEnumerable<Obstacle> obstacles )
        {
            this.obstacles.AddRange(obstacles);
        }
        public void AddObstacle(Obstacle obstacle)
        {
            this.obstacles.Add(obstacle);
        }
        public Vector3 GetMovePoint(Vector3 from, Vector3 to)
        {
            NavResult result = new NavResult(from, to);
            foreach (var o in obstacles)
                o.GetMovePoint(ref result);

            return result.movePoint;
        }
    }
}