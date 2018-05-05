using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Algoriphms;
namespace Assets.Other
{
    public static class TransformExtends
    {
        public static Transform[] childs(this Transform self)
        {
            int n = self.childCount;
            Transform[] c = new Transform[n];
            for (int i = 0; i < n; i++)
                c[i] = self.GetChild(i);
            return c;
        }
        public static bool IsOne(this Transform self, Transform root)
        {
            if (self == root)
                return true;
            var r = self.parent;
            while(r != null)
            {
                if (r == root)
                    return true;
                r = r.parent;
            }
            return false;
        }
        public static List<T> childs<T>(this Transform self) where T : MonoBehaviour
        {
            int n = self.childCount;
            List<T> c = new List<T>();
            for (int i = 0; i < n; i++)
            {
                var t = self.GetChild(i).GetComponent<T>();
                if (t != null)
                    c.Add(t);
            }
            return c;
        }
        public static T GetComponentInParents<T>(this Transform self) where T : MonoBehaviour
        {
            var p = self.parent;
            while(p != null)
            {
                T com = p.GetComponent<T>();
                if (com != null)
                    return com;
                else
                    p = p.parent;
            }
            return null;
        }
    }
}
