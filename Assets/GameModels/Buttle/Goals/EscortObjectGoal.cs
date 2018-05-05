using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameModels.Battle.Goals
{
    public class EscortObjectGoal : IBattleGoal
    {
        public readonly ShipController target;
        public readonly ShipController to;
        float sqrTriggerRadius;

        EscortBehaviour positive;
        AntiEscortBehaviour negative;

        public EscortObjectGoal(ShipController target, ShipController to, float triggerRadius)
        {
            this.target = target;
            this.to = to;
            this.sqrTriggerRadius = triggerRadius * triggerRadius;

            positive = new EscortBehaviour(this);
            negative = new AntiEscortBehaviour(this);
        }

        public bool Check()
        {
            return (to.tempTransform.position - target.tempTransform.position).sqrMagnitude < sqrTriggerRadius;
        }

        public AIBehabiour GetAntiBehaviour()
        {
            return negative;
        }

        public AIBehabiour GetBehaviour()
        {
            return positive;
        }
    }

    public class EscortBehaviour : AIBehabiour
    {
        EscortObjectGoal goal;
        public EscortBehaviour(EscortObjectGoal goal)
        {
            this.goal = goal;
            shipWeghts = new float[] { 0, .5f, .75f, 1, 1 };
        }

        protected override float GetGoalWeight(AIDriver driver)
        {
            return driver.ship == goal.target ? 1f :
                6250000f/*/ 2500 * 2500 /*/ / (goal.target.tempTransform.position - driver.ship.tempTransform.position).sqrMagnitude;
        }

        public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
        {
        }

        public override bool GetMovePoint(AIDriver driver, out Vector3 result)
        {
            if (driver.ship == goal.target)
            {
                result = goal.to.tempTransform.position;
                return true;
            }
            else {
                result = goal.target.tempTransform.position;
                return true;
            }
        }
    }

    public class AntiEscortBehaviour: AIBehabiour
    {
        EscortObjectGoal goal;
        public AntiEscortBehaviour(EscortObjectGoal goal)
        {
            this.goal = goal;
            shipWeghts = new float[] { 0, .75f, .5f, 1f, 1f };
        }

        protected override float GetGoalWeight(AIDriver driver)
        {
            return 6250000f/*/ 2500 * 2500 /*/ / (goal.target.tempTransform.position - driver.ship.tempTransform.position).sqrMagnitude;
        }

        public override void GetEnemies(AIDriver driver, int maxEnemies, ICollection<ShipController> outEnemiesResult)
        {
            outEnemiesResult.Add(goal.target);
        }

        public override bool GetMovePoint(AIDriver driver, out Vector3 result)
        {
            result = goal.target.tempTransform.position;
            return true;
        }
    }
}
