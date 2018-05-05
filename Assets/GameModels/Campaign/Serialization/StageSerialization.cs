using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using MyLib;

namespace Assets.GameModels.Campaign
{
    public partial class Campaign
    {
        public partial class Stage
        {
            [Version(1)]
            struct StageSerialization : IBinder
            {
                [ConstructorArg(0)]
                int FakeValue { get { return 1; } set { } } //For keep this struct

                [Addon]
                List<Stage> forwardStages { get { return stage.forwardStages; } set { stage.forwardStages = value; } }

                [Addon]
                List<int[]> __refStages { get { return stage._refStages.Select(s => s.GetPath()).ToList(); } set { stage.__refStages = value; } }

                [Addon]
                bool backAble { get { return stage.backAble; } set { stage.backAble = value; } }

                [Addon]
                LString actionString { get { return stage.actionString; } set { stage.actionString = value; } }
                [Addon]
                LString storyString {  get { return stage.storyString; } set { stage.storyString = value; } }

                [Addon]
                [SerializeBinder(typeof(NullBinder<BattleConfig>))]
                BattleConfig battleConfig { get { return stage.battleConfig; } set { stage.battleConfig = value; } }

                [Addon]
                List<ResourceAdder> adder { get { return stage.adder; } set { stage.adder = value; } }


                Stage stage;

                public StageSerialization(Stage stage)
                {
                    this.stage = stage;
                }

                public StageSerialization(int fake)
                {
                    this.stage = new Stage();
                }

                public object GetResult()
                {
                    return stage;
                }
            }
        }
    }
}
