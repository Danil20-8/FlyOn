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

        [Version(1)]
        struct Serializer : IBinder
        {
            [ConstructorArg(0)]
            string name { get { return campaign.name; } }

            [Addon]
            [SerializeBinder(typeof(DictionaryBinder<string, int>))]
            Dictionary<string, int> gpResources { get { return campaign.gpResources; } set { campaign.gpResources = value; } }

            [Addon]
            Stage.SerializeHelper shelper { get { return new Stage.SerializeHelper(campaign.rootStage); } set { campaign.rootStage = value.rootStage; } }
            [Addon]
            int[] _current { get { return campaign.current.GetPath(); } set { campaign.current = campaign.rootStage.GetNode(value); } }


            Campaign campaign;

            public Serializer(Campaign campaign)
            {
                this.campaign = campaign;
            }
            public Serializer(string name)
            {
                campaign = new Campaign(name);
            }
            public object GetResult()
            {
                return campaign;
            }
        }
    }
}
