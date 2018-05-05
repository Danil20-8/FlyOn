using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BattleEventUpdater
{
    public void AddEvent(Action action, int updatesToAction = 0, float timeToAction = 0)
    {
        new_events.Add(new UpdaterEvent(action, updatesToAction, timeToAction));
    }

    public void Update()
    {
        if (new_events.Count != 0)
        {
            var t = curr_events;
            curr_events = new_events;
            new_events = t;
            new_events.Clear();

            foreach (var e in curr_events)
            {
                if (e.updatesToAction == 0)
                {
                    if (e.timeToAction <= 0)
                        e.action();
                    else {
                        e.timeToAction -= Time.deltaTime;
                        new_events.Add(e);
                    }
                }
                else {
                    e.updatesToAction--;
                    new_events.Add(e);
                }
            }
        }
    }

    List<UpdaterEvent> curr_events = new List<UpdaterEvent>();
    List<UpdaterEvent> new_events = new List<UpdaterEvent>();
}

class UpdaterEvent
{
    public Action action;
    public int updatesToAction;
    public float timeToAction;
    public UpdaterEvent(Action action, int updatesToAction, float timeToAction)
    {
        this.action = action;
        this.updatesToAction = updatesToAction;
        this.timeToAction = timeToAction;
    }
}

