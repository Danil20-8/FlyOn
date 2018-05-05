using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MyLib.Serialization;
using UnityEngine;
using Assets.Global;

namespace Assets.Serialization
{
    public static class GameSerializer
    {
        static Encoding encoding { get { return Encoding.GetEncoding(int.Parse(GameConfig.Get("Encoding", "1251"))); } }
        public static void Serialize(object data, string path)
        {
            CompactSerializer cs = new CompactSerializer();
            using (StreamWriter sw = new StreamWriter(path))
            {
                cs.Serialize(data, sw);
            }
        }
        public static void Serialize(object data, string path, SerializeBinders binders)
        {
            CompactSerializer cs = new CompactSerializer(binders);
            using (StreamWriter sw = new StreamWriter(path))
            {
                cs.Serialize(data, sw);
            }
        }
        public static T Deserialize<T>(string path)
        {
            CompactSerializer cs = new CompactSerializer();
            using (StreamReader sr = new StreamReader(path, encoding))
            {
                return cs.Deserialize<T>(sr);
            }
        }
        public static T Deserialize<T>(string path, SerializeBinders binders)
        {
            CompactSerializer cs = new CompactSerializer(binders);
            using (StreamReader sr = new StreamReader(path, encoding))
            {
                return cs.Deserialize<T>(sr);
            }
        }
    }
}
