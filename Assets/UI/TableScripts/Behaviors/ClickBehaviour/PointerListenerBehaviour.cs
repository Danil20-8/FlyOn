using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Modern;
public class PointerListenerBehaviour : MonoBehaviour
{
    List<Tuple<Action<MonoBehaviour>, Type>> clickListeners = new List<Tuple<Action<MonoBehaviour>, Type>>();

    List<Tuple<Action<MonoBehaviour>, Type>> enterListeners = new List<Tuple<Action<MonoBehaviour>, Type>>();

    public void AddClickListener<T>(Action<MonoBehaviour> listener)
    {
        clickListeners.Add(new Tuple<Action<MonoBehaviour>, Type>(listener, typeof(T)));
    }

    public void AddEnterListener<T>(Action<MonoBehaviour> listener)
    {
        enterListeners.Add(new Tuple<Action<MonoBehaviour>, Type>(listener, typeof(T)));
    }

    public void OnClick(GameObject gameObject)
    {
        foreach(var l in clickListeners)
        {
            var b = (MonoBehaviour) gameObject.GetComponent(l.Item2);

            if (b != null)
                l.Item1(b);
        }
    }

    public void OnEnter(GameObject gameObject)
    {
        foreach (var l in enterListeners)
        {
            var b = (MonoBehaviour)gameObject.GetComponent(l.Item2);

            if (b != null)
                l.Item1(b);
        }
    }
}

