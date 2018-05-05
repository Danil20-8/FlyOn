using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Serialization;
using MyLib.Serialization.Binders;
using MyLib.Serialization;
using System.IO;
using UnityEngine;
namespace Assets.Global
{
    public static class GameConfig
    {
        static Dictionary<string, string> options;
        static GameConfig()
        {
            try {
                CompactSerializer cs = new CompactSerializer();
                using (StreamReader sr = new StreamReader("GameConfig.txt"))
                {
                    options = cs.Deserialize<Dictionary<string, string>>(sr, typeof(DictionaryBinder<string, string>));
                }
            }
            catch
            {
                options = new Dictionary<string, string>();
            }
        }
        public static string Get(string option, string defaultValue)
        {
            string r;
            if (options.TryGetValue(option, out r))
                return r;
            else
            {
                options.Add(option, defaultValue);
                Save();
                return defaultValue;
            }
        }
        public static string Get(string option)
        {
            return options[option];
        }
        public static void Set(string option, string value)
        {
            options[option] = value;
        }
        public static void Save()
        {
            CompactSerializer cs = new CompactSerializer();
            using (StreamWriter sw = new StreamWriter("GameConfig.txt"))
            {
                cs.Serialize(options, sw, typeof(DictionaryBinder<string, string>));
            }
        }
    }
}
