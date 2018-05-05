using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;

[RequireComponent(typeof(Canvas))]
public class GuideBehaviour : MonoBehaviour
{
    [SerializeField]
    TutorialDialog dialog;

    Queue<GuideData> guide;
    string guideName = "ShipTutorial";

    void Start()
    {
        if(!bool.Parse(GameConfig.Get(guideName, "true")))
        {
            Destroy(gameObject);
            return;
        }

        dialog.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        guide = new Queue<GuideData>();
        GuideData[] tasks = new GuideData[]
        {
            new GuideData() { text = "WellcomToTheGame", shortText = "AnyKey", task = PressAnyKey },
            new GuideData() { text = "ChangeFlyMode", shortText = "PressF1", task = ChangeFlyMod },
        };

        foreach (var g in tasks)
            guide.Enqueue(g);

        NextGuide();
    }
    void NextGuide()
    {
        if (guide.Count > 0)
        {
            var g = guide.Dequeue();
            dialog.SetTutorial(Localization.GetGlobalString(g.text), g.task, NextGuide);
        }
        else
        {
            GameConfig.Set(guideName, false.ToString());
            GameConfig.Save();
            Destroy(gameObject);
        }
    }

    bool PressAnyKey()
    {
        return Input.anyKeyDown;
    }

    bool ChangeFlyMod()
    {
        return Input.GetKeyDown(KeyCode.F1);
    }
}

struct GuideData
{
    public string text;
    public string shortText;
    public Func<bool> task;
}
