using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Assets.Other;

namespace Assets.Global
{
    public static class GameState
    {
        public static GlobalUpdater updater;

        static string levelsFolder = @"Assets\Levels\levels.txt";
        static string current = "ShipTable";
        public static LoadingInfo loadingInfo = new LoadingInfo();

        static string levels { get { return Resources.Load<TextAsset>("levels").text; } }

        public static readonly MyDictionary<string, object> sharedData = new MyDictionary<string, object>();

        public static void GoToAnchor()
        {

        }

        public static void GoToSpace()
        {
            Cursor.visible = false;
            GoTo("Space");
        }

        public static void GoToBattle(BattleConfig config)
        {
            sharedData["BattleConfig"] = config;

            GoToSpace();
        }

        public static void GoToShipBuilder()
        {
            Cursor.visible = true;
            GoTo("ShipBuilder");
        }
        public static void GoToMainMenu()
        {
            Cursor.visible = true;
            GoTo("ShipBuilder");
        }
        public static void GoTo(string level)
        {
            SceneManager.LoadScene(KeyString.GetString(level, levels));
        }

        static string GetPath(params string[] path)
        {
            return String.Join(@"\", new string[] { Environment.CurrentDirectory }.Concat(path).ToArray());
        }
    }
    class KeyString
    {
        /*public static string GetString(string key, string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                while (!r.EndOfStream)
                {
                    Match m = Regex.Match(r.ReadLine(), key + " = \"(.+?)\"");
                    if (m.Success)
                        return m.Result("$1");
                }
            }
            return "";
        }*/
        public static string GetString(string key, string source)
        {
            Match m = Regex.Match(source, key + " = \"(.+?)\"");
            if (m.Success)
                return m.Result("$1");

            return "";
        }
    }
}

public class LoadingInfo
{
    public Func<float> GetProgress;
    public string info;

    public event Action waitingEnd;

    public void EndLoading()
    {
        waitingEnd();
    }
}

public interface ILoading
{
    void OnLoad();
}