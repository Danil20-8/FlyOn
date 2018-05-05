using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Global
{
    public class GamePool
    {
        Dictionary<string, Stack<MonoBehaviour>> pool = new Dictionary<string, Stack<MonoBehaviour>>();
        Dictionary<string, MonoBehaviour> loaded = new Dictionary<string, MonoBehaviour>();
        public  T Get<T>(string path, Vector3 position, Quaternion rotation, Transform parent)where T : MonoBehaviour
        {
            Stack<MonoBehaviour> p;
            if (pool.TryGetValue(path, out p))
            {
                if (p.Count > 0)
                {
                    var r = (T)p.Pop();
                    r.transform.position = position;
                    r.transform.rotation = rotation;
                    r.transform.parent = parent;
                    r.gameObject.SetActive(true);
                    return r;
                }
                else
                    return (T)GameObject.Instantiate(loaded[path], position, rotation, parent);
            }
            else
            {
                var a = Resources.Load<T>(path);
                loaded.Add(path, a);
                pool.Add(path, new Stack<MonoBehaviour>());
                return (T)GameObject.Instantiate(a, position, rotation);
            }

        }
        public void Put<T>(string path, T obj)where T : MonoBehaviour
        {
            obj.gameObject.SetActive(false);
            pool[path].Push(obj);
        }
        public void Clear()
        {
            foreach (var p in pool)
                foreach (var o in p.Value)
                    GameObject.Destroy(o.gameObject);
            pool.Clear();

            loaded.Clear();
        }
    }

    public interface IPoolObject
    {
        void Destroy();
    }
}
