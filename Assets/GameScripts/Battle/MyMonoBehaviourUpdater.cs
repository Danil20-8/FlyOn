using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;

public class MyMonoBehaviourUpdater : IBattleUpdater, IAddShipAble, IRemoveShipAble
{
    List<IMyMonoBehaviour> behaviours;

    List<IMyMonoBehaviour> toAdd = new List<IMyMonoBehaviour>();
    List<IMyMonoBehaviour> toRemove = new List<IMyMonoBehaviour>();

    public void AddBehaviour(IMyMonoBehaviour behaviour)
    {
        toAdd.Add(behaviour);
    }

    public void RemoveBehaviour(IMyMonoBehaviour behaviour)
    {
        toRemove.Add(behaviour);
    }

    public T GetBehaviour<T>() where T: IMyMonoBehaviour
    {
        foreach (var b in behaviours)
            if (b is T)
                return (T)b;

        throw new Exception("Behaviour's not found");
    }

    public MyMonoBehaviourUpdater(IEnumerable<IMyMonoBehaviour> behaviours)
    {
        this.behaviours = behaviours.ToList();
    }
    public void Update()
    {
        foreach (var b in toRemove)
            behaviours.Remove(b);

        foreach (var b in toAdd)
            behaviours.Add(b);

        toAdd.Clear();
        toRemove.Clear();

        foreach (var b in behaviours)
            b.__InitializeUpdate();
        
        PRun.Split(behaviours.Count, (left, right) =>
        {
            for(int i = left; i < right; i++)
                behaviours[i].__FastUpdate();
        });
        
        foreach (var b in behaviours)
            b.__SlowUpdate();
    }

    public void AddShip(ShipController ship)
    {
        AddBehaviour(ship);
    }

    public void RemoveShip(ShipController ship)
    {
        RemoveBehaviour(ship);
    }
}

