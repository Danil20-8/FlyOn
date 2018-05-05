using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using MyLib;
using MyLib.Algoriphms;
namespace Assets.GameModels.Phisical
{
    public class HullModel
    {
        [ConstructorArg(1)]
        [SerializeBinder(typeof(DictionaryBinder<int, int[]>))]
        Dictionary<int, int[]> tackles = new Dictionary<int, int[]>();
        [ConstructorArg(0)]
        public readonly string hullName;

        public HullModel(string name, Dictionary<int, int[]> tacklePlace)
        {   
            hullName = name;
            tackles = tacklePlace;
        }

        public Dictionary<int, SystemLink> GetTackles(Forest<SystemLink> links)
        {
            return tackles.ToDictionary(p => p.Key, p => links.GetNode(p.Value));
        }
    }
}
