using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using Assets.Other.Special;
using System;

namespace Assets.Models.Pool
{

    public class GameObject<T> : IPoolable<GameObject<T>> where T : UnityEngine.Component
    {
        public T obj;
        Transform parent;
        public GameObject(T obj, Transform parent = null)
        {
            this.obj = obj;
            this.parent = parent;
        }

        GameObject<T> ICloneable<GameObject<T>>.Clone()
        {
            return new GameObject<T>(GameObject.Instantiate(obj, parent) as T, parent);
        }

        void IPoolable<GameObject<T>>.Destroy()
        {
            GameObject.Destroy(obj);
        }

        void IPoolable<GameObject<T>>.Disable()
        {
            obj.gameObject.SetActive(false);
        }

        void IPoolable<GameObject<T>>.Reset()
        {
            obj.gameObject.SetActive(true);
        }
    }
}
