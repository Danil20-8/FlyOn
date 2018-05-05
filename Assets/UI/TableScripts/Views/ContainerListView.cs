using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Other;
public class ContainerListView : ListView{

    [SerializeField]
    ContainerItemView view;
    public void AddItem(object item)
    {
        var v = Instantiate(view);
        v.SetModel(item);
        AddItem(v);
    }
    public void AddItem(object item, Action<ContainerItemView> action)
    {
        var v = Instantiate(view);
        v.SetModel(item);
        AddItem(v);
        action(v);
    }
    public void AddRange<T>(IEnumerable<T> items)
    {
        AddRange(items.Select(i => { var v = (ContainerItemView) Instantiate(view, content); v.SetModel(i); return v; }));
    }
    public void AddItem(ContainerItemView item)
    {
        base.AddItem(item.transform);
    }
    public void AddRange(IEnumerable<ContainerItemView> items)
    {
        base.AddRange(items.Select(i => i.transform));
    }

    new public ContainerItemView GetItem(int index)
    {
        return base.GetItem(index).GetComponent<ContainerItemView>();
    }

    public void Order<T>(Func<T, float> keySelector)
    {
        this.keySelector = t => keySelector(t.GetComponent<ContainerItemView>().GetModel<T>());
        Refresh();
    }
}
