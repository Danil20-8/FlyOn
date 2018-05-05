using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameModels;
using Assets.Serialization;
using MyLib.Serialization;
using MyLib.Serialization.Binders;
using UnityEngine;
namespace Assets.Global
{
    public static class PlayerResources
    {
        public static Inventory inventory { get { return player.inventory; } }
        public static ShipsInventory ships { get { return player.ships; } }
        public static Player player { get; private set; }
        static PlayerResources()
        {
            try
            {
                Load();
            }
            catch
            {
                player = new Player("Player");
            }
        }

        public static void Save()
        {
            Save(string.Join(@"\", new string[] { Environment.CurrentDirectory, "save.txt" }));
        }
        public static void Load()
        {
            Load(string.Join(@"\", new string[] { Environment.CurrentDirectory, "save.txt" }));
        }
        public static void Save(string path)
        {
            GameSerializer.Serialize(player, path);
        }
        public static void Load(string path)
        {
            player = GameSerializer.Deserialize<Player>(path);
        }
    }
}
