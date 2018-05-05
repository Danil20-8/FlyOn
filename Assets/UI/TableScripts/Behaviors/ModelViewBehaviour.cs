using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;
using UnityEngine;

public class ModelViewBehaviour: MyMonoBehaviour
{
    Dictionary<object, List<ContainerItemView>> dict = new Dictionary<object, List<ContainerItemView>>();

    public void AddModelView(object model, ContainerItemView view)
    {
        List<ContainerItemView> views;
        if(dict.TryGetValue(model, out views))
        {
            views.Add(view);
        }
        else
        {
            dict.Add(model, new List<ContainerItemView>() { view });
        }
    }
    public void RemoveView(object model, ContainerItemView view)
    {
        var l = dict[model];
        l.Remove(view);
        if (l.Count == 0)
            dict.Remove(model);
    }
    public T GetView<T>(object model) where T : ContainerItemView
    {
        List<ContainerItemView> views;
        if (dict.TryGetValue(model, out views))
        {
            var r = (T)views.FirstOrDefault(v => v is T);
            if (r != null)
                return r;
        }

        throw new Exception("No view presents " + model);
    }

    public T[] GetAll<T>(object model) where T : ContainerItemView
    {
        List<ContainerItemView> views;
        if (dict.TryGetValue(model, out views))
        {
            var r = views.Where(v => v is T).Cast<T>().ToArray();
            if (r.Length > 0)
                return r;
        }

        throw new Exception("No view presents " + model);
    }

    public T GetView<T>(string label) where T : ContainerItemView
    {
        var r = (T) dict.SelectMany(p => p.Value).FirstOrDefault(v => v.labelItem == label);

        if (r != null)
            return r;

        throw new Exception("No view with label " + label);
    }
}

