using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MessageListener : MonoBehaviour
{
    Dictionary<Type, List<IListener>> listeners = new Dictionary<Type, List<IListener>>();

    public void Send(object sender, object message)
    {
        List<IListener> ls;

        if (listeners.TryGetValue(message.GetType(), out ls))
            foreach (var l in ls)
                l.Invoke(sender, message);
    }

    public void AddListener<T>(Action<object, T> listener)
    {
        List<IListener> ls;

        if (listeners.TryGetValue(typeof(T), out ls))
            ls.Add(new Listener<T>(listener));
        else
        {
            ls = new List<IListener>() { new Listener<T>(listener) };
            listeners.Add(typeof(T), ls);
        }
    }

    public void RemoveListener<T>(Action<object, T> listener)
    {
        List<IListener> ls;

        if (listeners.TryGetValue(typeof(T), out ls))
            for (int i = 0; i < ls.Count; i++)
                if (ls[i].Equals(listener))
                    ls.RemoveAt(i);
    }

    interface IListener
    {
        void Invoke(object sender, object message);
    }
    struct Listener<T>: IListener
    {
        public Action<object, T> listener;

        public Listener (Action<object, T> listener)
        {
            this.listener = listener;
        }

        public void Invoke(object sender, object message)
        {
            listener(sender, (T)message);
        }

        public override bool Equals(object obj)
        {
            return listener.Equals(obj);
        }
    }
}


