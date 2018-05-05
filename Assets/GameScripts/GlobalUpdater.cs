using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using Assets.Global;

public class GlobalUpdater: MonoBehaviour
{
    List<Stage> _stages = new List<Stage>();

    List<Action> events_c = new List<Action>();
    List<Action> events_n = new List<Action>();

    public Stage[] stages { get { return _stages.ToArray(); } }

    public void AddEvent(Action action)
    {
        events_n.Add(action);
    }

    public void AddStage<T>() where T : Stage, new()
    {
        var s = new T();
        AddStage(s);
    }

    public void AddStage(Stage stage)
    {
        AddEvent(() => { _stages.Add(stage); stage.Init(); });
    }
    public void RemoveStage(Stage stage)
    {
        AddEvent(() => { if(_stages.Remove(stage)) stage.Destroy(); });
    }
    public void SetStages(IEnumerable<Stage> stages)
    {
        var ss = stages.ToArray();
        AddEvent(() => { this._stages.Clear(); this._stages.AddRange(ss); });
        AddEvent(() => { foreach (var s in ss) s.Init(); });
    }

    void Update()
    {
        var t = events_c;
        events_c = events_n;
        events_n = t;
        events_n.Clear();

        foreach (var e in events_c)
            e();

        foreach (var s in _stages)
            s.Update();
    }

    void Awake()
    {
        GameState.updater = this;
    }
}

public abstract class Stage
{
    public event Action onDestroy;

    public virtual void Init()
    {

    }
    public void Destroy()
    {
        OnDestroy();

        if(onDestroy != null)
            onDestroy();
    }

    protected virtual void OnDestroy()
    {

    }
    public abstract void Update();
}