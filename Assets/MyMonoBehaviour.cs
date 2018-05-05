using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class MyMonoBehaviour: MonoBehaviour, IMyMonoBehaviour
{
    Dictionary<Type, PBehaviour> components = new Dictionary<Type, PBehaviour>();
    Dictionary<Type, PBehaviour> newComponents = new Dictionary<Type, PBehaviour>();
    bool dirty = true;
    PBehaviour[] cache;

    public T AddMyComponent<T>() where T : PBehaviour
    {
        T component = Activator.CreateInstance<T>();
        newComponents.Add(typeof(T), component);
        component.behaviour = this;
        dirty = true;

        return component;
    }

    public T GetMyComponent<T>() where T : PBehaviour
    {
        PBehaviour component;
        if(components.TryGetValue(typeof(T), out component))
        {
            return (T)component;
        }

        throw new Exception("Component's not found");
    }

    public void Dirty()
    {
        dirty = true;
    }

    public void __InitializeUpdate()
    {
        if (dirty)
        {
            foreach (var c in newComponents)
                components.Add(c.Key, c.Value);
            foreach (var c in newComponents)
                c.Value.Start();
            newComponents.Clear();

            cache = components.Where(p => p.Value.enabled).Select(p => p.Value).ToArray();
            dirty = false;
        }
        foreach (var c in cache)
            c.InitializeUpdate();

        InitializeUpdate();
    }

    public void __FastUpdate()
    {
        foreach (var c in cache)
            c.FastUpdate();

        FastUpdate();
    }

    public void __SlowUpdate()
    {
        foreach (var c in cache)
            c.SlowUpdate();

        SlowUpdate();
    }

    protected virtual void InitializeUpdate()
    {

    }

    protected virtual void FastUpdate()
    {

    }

    protected virtual void SlowUpdate()
    {

    }
}

public abstract class PBehaviour
{
    MyMonoBehaviour _behaviour;
    public MyMonoBehaviour behaviour { get { return _behaviour; } set { if (_behaviour != null) throw new Exception("behaviour already seted"); _behaviour = value; } }
    bool _enabled = true;
    public bool enabled { get { return _enabled; } set { _enabled = value; _behaviour.Dirty(); } }
    public T GetComponent<T>() where T : PBehaviour
    {
        return behaviour.GetMyComponent<T>();
    }

    public virtual void Start()
    {

    }

    public abstract void InitializeUpdate();
    public abstract void FastUpdate();
    public abstract void SlowUpdate();
}

public interface IMyMonoBehaviour
{
    void __InitializeUpdate();

    void __FastUpdate();

    void __SlowUpdate();
}
