using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using Assets.Global;
using MyLib;
namespace Assets.GameModels.Campaign
{
    public partial class Campaign
    {
        public string name { get; private set; }

        Dictionary<string, int> gpResources = new Dictionary<string, int>();

        public ResourceAdder[] resources { get { return gpResources.Select(r => new ResourceAdder(r.Key, new ResourceValue(true, ResourceValueSource.Const, r.Value))).ToArray(); } }

        public string[] kinds { get { return gpResources.Keys.ToArray(); } set { gpResources = value.ToDictionary(s => s, s => 0); } }

        public Stage current { get; private set; }

        Stage rootStage;

        public Campaign(string name)
        {
            this.name = name;

            rootStage = new Stage();
            this.current = rootStage;
        }
    }
}
