using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;
using Assets.GameModels.Phisical;
using Assets.GameModels.Components;
using MyLib;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using MyLib.Modern;
using MyLib.Algoriphms;
namespace Assets.GameModels
{
    public class ShipSystemModel
    {
        [ConstructorArg(0)]
        public readonly string name;
        [ConstructorArg(1)]
        public readonly Forest<TreeNode<SystemComponent>> system;
        [ConstructorArg(2)]
        [SerializeBinder(typeof(DictionaryBinder<string, int[]>))]
        public readonly Dictionary<string, int[]> keys;
        [ConstructorArg(3)]
        public readonly HullModel hull;
        public ShipSystemModel(string name, Forest<TreeNode<SystemComponent>> system, Dictionary<string, int[]> keys, HullModel hull)
        {
            this.name = name;
            this.system = system;
            this.keys = keys;
            this.hull = hull;
        }
        public void GetKeys<T>(Forest<T> tree, Action<T, string> action) where T : ITreeable<T>
        {
            foreach (var k in keys)
                action(tree.GetNode(k.Value), k.Key);
        }
    }
    public class ShipSystem
    {
        public readonly ShipSystemModel model;
        public string name { get { return model.name; } }
        public readonly PowerLink[] power;
        public readonly Forest<SystemLink> system;
        public Dictionary<string, SystemLink> keys { get {
                var keys = new Dictionary<string, SystemLink>();
                model.GetKeys(system, (l, k) => keys.Add(k, l));
                return keys;
            } }
        public bool alive { get { foreach (var p in power) if (p.alive) return true; return false; } }
        public ShipSystem(ShipSystemModel model)
        {
            this.model = model;
            system = model.system.BuildForest(n => SystemLink.Pack(n.item));
            power = system.trees.Select(n => (PowerLink)n).ToArray();
            
        }
    }
}
