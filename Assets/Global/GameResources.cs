using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Models;
using UnityEngine;
using Assets.GameModels;
using Assets.Serialization;
using System.IO;
using MyLib.Serialization;
using MyLib.Algoriphms;

namespace Assets.Global
{
    public static class GameResources
    {
        static string customResources = string.Join(@"\", new string[] { Directory.GetCurrentDirectory(), "CustomResources" });
        static string GetPath(string path) { return string.Join(@"\", new string[] { customResources, path }); }


        public static ColorTheme colorTheme { get { return colorThemeObserver.colorTheme; } }
        public static ColorThemeObserver colorThemeObserver { get; private set; }
        static GameResources()
        {
            colorThemeObserver = new ColorThemeObserver( new ColorTheme(""));
        }

        public static HullBehaviour GetShipHull(string name)
        {
            return Resources.Load<HullBehaviour>(@"Hulls\" + name);
        }
        public static HullBehaviour[] GetAllShiphulls()
        {
            return Resources.LoadAll<HullBehaviour>("Hulls");
        }
        public static ShipComponentBehaviour GetShipComponent(string name)
        {
            return Resources.Load<ShipComponentBehaviour>(@"Components\" + name);
        }
        public static ShipSystemModel GetShipSystemModel(string name)
        {
            var inAsset = Resources.Load<TextAsset>(@"Ships\" + name);
            if (inAsset != null)
                return new CompactSerializer().Deserialize<ShipSystemModel>(inAsset.text);
            else
                return Load<ShipSystemModel>(@"Ships\" + name + ".txt");
        }
        public static ShipSystemModel[] GetAllShips()
        {
            var cs = new CompactSerializer();

            return Resources.LoadAll<TextAsset>("Ships").Select(t => cs.Deserialize<ShipSystemModel>(t.text))
                .Concat(LoadAll("Ships", "txt", sr => cs.Deserialize<ShipSystemModel>(sr)))
                .ToArray();
        }

        public static string[] GetShipsNames()
        {
            return GetFiles("Ships", "txt").Select(n => n.Split('.')[0].Split('\\').Last()).ToArray();
        }

        static string[] GetFiles(string path, string extend)
        {
            path = GetPath(path);
            return Directory.Exists(path) ? Directory.GetFiles(path).Where(n => n.Split('.').Last() == extend).ToArray()
                : new string[0];
        }

        public static IEnumerable<T> LoadAll<T>(string path, string extend, Func<StreamReader, T> selector)
        {
            return GetFiles(path, extend)
                .Select(n =>
                {
                    using (StreamReader sr = File.OpenText(n))
                    {
                        return selector(sr);
                    }
                });
        }
        public static T Load<T>(string path, Func<StreamReader, T> loader)
        {
            using (StreamReader sr = File.OpenText(GetPath(path)))
            {
                return loader(sr);
            }
        }

        public static IEnumerable<T> LoadAll<T>(string path, string extend)
        {
            var cs = new CompactSerializer();

            return LoadAll(path, extend, sr => cs.Deserialize<T>(sr));
        }
        public static T Load<T>(string path)
        {
            using (StreamReader sr = File.OpenText(GetPath(path)))
            {
                return new CompactSerializer().Deserialize<T>(sr);
            }
        }
        public static void Save(string path, object data, Func<bool> fileExistsDiolog)
        {
            var cs = new CompactSerializer();

            path = GetPath(path);

            var dir = new string(path.Reverse().SkipWhile(c => c != '\\').Reverse().ToArray());
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(path))
                if (!fileExistsDiolog())
                    return;

            using (StreamWriter sw = File.CreateText(path))
            {
                cs.Serialize(data, sw);
            }
        }

        public static byte[] Load(string path)
        {
            path = GetPath(path);

            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }
    }
}
