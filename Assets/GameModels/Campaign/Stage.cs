using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib;
using MyLib.Serialization;
using MyLib.Serialization.Binders;

using Assets.Global;

namespace Assets.GameModels.Campaign
{
    public partial class Campaign
    {

        public partial class Stage : ITreeable<Stage>
        {

            public List<Stage> forwardStages { get { return _forwardStages; } set { _forwardStages = value; foreach (var s in _forwardStages) s.preview = this; } }
            List<Stage> _forwardStages;

            Stage preview;


            public HashSet<Stage> refStages { get { return _refStages; } }
            HashSet<Stage> _refStages = new HashSet<Stage>();
            //For serialize helper
            List<int[]> __refStages;

            public bool backAble { get; set; }

            Stage back = null;

            public Stage root { get { return preview; } set { preview = value; } }

            public IEnumerable<Stage> childs{get{return forwardStages;}}

            public LString actionString;

            public LString storyString;

            public bool battle { get { return battleConfig != null; } }

            public BattleConfig battleConfig = null;

            public List<ResourceAdder> adder = new List<ResourceAdder>();

            public List<StageCondition> conditions = new List<StageCondition>();

            public Stage()
            {
                _forwardStages = new List<Stage>();
                actionString = new LString();
                storyString = new LString();
            }

            public void AddChild(Stage child)
            {
                forwardStages.Add(child);
            }

            public void RemoveChild(Stage child)
            {
                forwardStages.Remove(child);
            }

            public bool Transit(BattleInfo info, Campaign campaign)
            {
                if (!IsAble(info, campaign))
                    return false;

                foreach (var a in adder)
                {
                    if (a.value.source == ResourceValueSource.Const)
                        campaign.gpResources[a.name] += a.value.amount;
                    else
                        campaign.gpResources[a.name] += a.value[info];
                }
                if(campaign.current.back != this)
                    back = campaign.current;
                campaign.current = this;

                return true;
            }
            public bool IsAble(BattleInfo info, Campaign campaign)
            {
                foreach (var c in conditions)
                    if (!(c.isPass(info, campaign.gpResources)))
                        return false;
                return true;
            }

            public class SerializeHelper
            {
                [ConstructorArg(0)]
                public Stage rootStage;

                public SerializeHelper(Stage rootStage)
                {
                    this.rootStage = rootStage;
                }
                [PostDeserialize]
                void EndDeserialize()
                {
                    foreach (var s in rootStage.ByElements())
                        s._refStages = new HashSet<Stage>(s.__refStages.Select(r => rootStage.GetNode(r)));
                }
            }
        }
    }

    [Serializable]
    public struct ResourceAdder
    {
        public string name;
        public ResourceValue value;

        public ResourceAdder(string name, ResourceValue value)
        {
            this.name = name;
            this.value = value;
        }
    }

    [Serializable]
    public struct ResourceValue
    {
        public bool positive;
        public ResourceValueSource source;
        public int this[BattleInfo info] {
            get
            {
                int result = 0;
                switch(source)
                {
                    case ResourceValueSource.Const:
                        result = amount;
                        break;
                    case ResourceValueSource.MyDeadShipsAmount:
                        result = info.GetDeadShipsAmount(info.teams.First(t => t.myTeam));
                        break;
                    case ResourceValueSource.EnemyDeadShipsAmount:
                        result = info.GetDeadShipsAmount(info.teams.First(t => !t.myTeam));
                        break;
                }
                return positive ? result : -result;
            }
        }
        public int amount;

        public ResourceValue(bool positive, ResourceValueSource source, int amount = 0)
        {
            this.source = source;
            this.amount = amount;
            this.positive = positive;
        }

        public ResourceValue(int amount)
        {
            this.source = ResourceValueSource.Const;
            this.amount = Math.Abs(amount);
            this.positive = amount >= 0;
        }

        public static implicit operator ResourceValue(int amount)
        {
            return new ResourceValue(amount);
        }
    }

    [Serializable]
    public enum ResourceValueSource
    {
        Const,
        MyDeadShipsAmount,
        EnemyDeadShipsAmount
    }
}
