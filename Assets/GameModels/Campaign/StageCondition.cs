using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Global;

namespace Assets.GameModels.Campaign
{

    public class StageCondition
    {
        public BattleResultRequired battleResult;

        public Dictionary<string, int> need = new Dictionary<string, int>();

        public bool isPass(BattleInfo info, Dictionary<string, int> resources)
        {
            if (battleResult != BattleResultRequired.Any)
            {
                if (info == null)
                    throw new ArgumentNullException("BattleInfo");
                if ((info.teams.First(t => t.myTeam) == info.winner && battleResult == BattleResultRequired.Defeat) || (info.teams.First(t => t.myTeam) != info.winner && battleResult == BattleResultRequired.Victory))
                {
                    return false;
                }
            }

            foreach (var r in need)
                if (resources[r.Key] < r.Value)
                    return false;

            return true;
        }

    }

    [Serializable]
    public enum BattleResultRequired
    {
        Any,
        Victory,
        Defeat
    }
}
