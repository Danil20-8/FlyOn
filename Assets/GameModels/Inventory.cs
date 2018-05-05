using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Assets.GameModels.Components;
using System.Reflection;
using MyLib.Serialization;

namespace Assets.GameModels
{
    public class Inventory
    {
        [Addon]
        List<SystemComponent> items = new List<SystemComponent>();
        public IEnumerable<SystemComponent> Items { get { return items; } }

        public Inventory()
        {
            //Remove it before release game
            items.AddRange(Enumerable.Range(0, 5).SelectMany(i => Enumerable.Range(0, 5).SelectMany(n => GetExamples(i))));
        }

        public void AddItem(SystemComponent item)
        {
            items.Add(item);
        }

        public void DropItem(SystemComponent item)
        {
            if (!items.Remove(item))
                throw new Exception("Inventory does not contain item");
        }
        public void DropItems(params SystemComponent[] itemsRange)
        {
            foreach (var i in itemsRange)
                if (items.IndexOf(i) == -1)
                    throw new Exception("Inventory does not contain items");
            foreach (var i in itemsRange)
                items.Remove(i);
        }

        ulong Sum()
        {
            ulong cs = 0;
            foreach (var i in items)
            {
                var c = i.GetConfig();
                ulong j = 0;
                foreach (var n in c.GetNames())
                {
                    cs += j + (ulong)Math.Abs(c.GetField(n).GetHashCode());
                    j = cs / 2 + 1;
                }
            }
            return cs;
        }

        public static IEnumerable<SystemComponent> GetExamples(int shipClass)
        {
            return new SystemComponent[]
            {
                new GunComponent(shipClass),
                new FreezeRayComponent(shipClass),
                new RocketGunComponent(shipClass),
            };
        }
        public static IEnumerable<SystemComponent> GetBaseExamples(int shipClass)
        {
            return new SystemComponent[]
            {
                new PowerComponent(shipClass),
                new EngineComponent(shipClass),
            };
        }
    }
}
