using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using Assets.Global;
using System.Collections;
using Assets.Other.Special;
using MyLib.Algoriphms;

namespace Assets.GameModels.Battle
{
    public class BattleArea : BattleArea<ShipController>, IAddShipAble, IDestroyShipAble
    {
        public BattleArea(float size, float areaSize, Action<ShipController> outOfBounds)
            : base(size, areaSize, outOfBounds)
        { }

        public void AddShip(ShipController ship)
        {
            AddObject(ship);
        }

        void IDestroyShipAble.DestroyShip(ShipController ship)
        {
            ReleaseObject(ship);
        }
    }

    public class BattleArea<T> where T : ISpatialPartionable
    {
        Part[] parts;
        List<Part> actives = new List<Part>();
        ObjectPool<Data> objects = new ObjectPool<Data>(new Data(), 2);

        Action<T> outOfBounds;

        float size;
        float halfSize;
        float areaSize;
        float sizeDivCount;
        float countDivSize;
        float areaRadius;
        int count;
        int offset;
        int doubleOffset;
        uint mask;
        public BattleArea(float size, float areaSize, Action<T> outOfBounds)
        {
            this.size = size;
            halfSize = size * .5f;
            this.areaSize = areaSize;
            count = (int)(size / areaSize + .5f);
            sizeDivCount = size / count;
            countDivSize = ((float)count) / size;
            areaRadius = Mathf.Sqrt(areaSize * areaSize * .75f); // sqrt((areaSize  / 2) * (areaSize / 2) * 3)
            float foffset = Mathf.Log(size / areaSize, 2);
            offset = foffset - (int)foffset == 0 ? (int)foffset : (int)foffset + 1;
            doubleOffset = offset * 2;
            if (offset > 10)
                throw new Exception("Too much of area parts");
            mask = (uint)Mathf.Pow(2, offset);
            parts = new Part[mask * mask * mask];
            mask -= 1;
            var cubeCenter = new Vector3(areaSize / 2, areaSize / 2, areaSize / 2);
            for (uint i = 0; i < parts.Length; i++)
            {
                parts[i] = new Part(IntToVector3(i) + cubeCenter);
            }

            this.outOfBounds = outOfBounds;
        }
        Vector3 IntToVector3(uint value)
        {
            return new Vector3(
                (value & mask) * sizeDivCount - halfSize,
                ((value >> offset) & mask) * sizeDivCount - halfSize,
                ((value >> doubleOffset) & mask) * sizeDivCount - halfSize);
        }
        bool uintPint(uint s, int i, out uint r)
        {
            if (i >= 0)
            {
                r = s + (uint)i;
                return true;
            }
            else
            {
                uint t = (uint)(i * -1);
                if (t > s)
                {
                    r = 0;
                    return false;
                }
                else
                {
                    r = s - t;
                    return true;
                }
            }
        }
        uint GetIndex(uint x, uint y, uint z)
        {
            return x + (y << offset) + (z << doubleOffset);
        }
        uint Vector3ToInt(Vector3 vector)
        {
            uint x = (uint)((vector.x + halfSize) * countDivSize + .5f);
            uint y = (uint)((vector.y + halfSize) * countDivSize + .5f);
            uint z = (uint)((vector.z + halfSize) * countDivSize + .5f);
            return x + (y << offset) + (z << doubleOffset);
        }
        bool Vector3ToInt(Vector3 vector, out uint result)
        {
            uint x = (uint)((vector.x + halfSize) * countDivSize + .5f);
            uint y = (uint)((vector.y + halfSize) * countDivSize + .5f);
            uint z = (uint)((vector.z + halfSize) * countDivSize + .5f);
            if (x < size && y < size && z < size)
            {
                result = x + (y << offset) + (z << doubleOffset);
                return true;
            }
            result = 0;
            return false;
        }
        Part GetArea(Vector3 position)
        {
            return parts[Vector3ToInt(position)];
        }
        bool GetArea(Vector3 position, out Part result)
        {
            uint index;
            if (!Vector3ToInt(position, out index))
            {
                result = null;
                return false;
            }
            result = parts[index];
            return true;
        }
        public void GetObjects(Vector3 position, float radius, ICollection<T> outObjects)
        {
            outObjects.Clear();
            float sqrRadius = radius * radius;
            float doubleRadius = radius * 2;
            float searchRadius = radius + areaRadius;
            int count = actives.Count;
            Part active;
            float distance;
            FastList<Data> t_datas;
            Data data;
            for (int i = 0; i < actives.Count; i++)
            {
                active = actives[i];
                distance = (active.position - position).magnitude;

                if (distance < searchRadius)
                {
                    if (doubleRadius > searchRadius)
                    {
                        t_datas = active.objects;
                        float length = t_datas.Count;
                        for (int j = 0; j < length; j++)
                            outObjects.Add(t_datas[j].obj);
                    }
                    else {
                        t_datas = active.objects;
                        float length = t_datas.Count;
                        for (int j = 0; j < length; j++)
                        {
                            data = t_datas[j];
                            if ((data.position - position).sqrMagnitude < sqrRadius)
                                outObjects.Add(data.obj);
                        }
                    }
                }
            }
        }

        public void AddObjects(IEnumerable<T> objects)
        {
            foreach (var s in objects)
                AddObject(s);
        }

        public void AddObject(T obj)
        {
            var area = GetArea(obj.position);
            if (area.empty)
                actives.Add(area);

            objects.Get().Set(obj, area); //Go!
        }
        public void UpdateObjects()
        {
            foreach (var data in objects)
            {
                Part area;
                if (!GetArea(data.position, out area))
                {
                    outOfBounds(data.obj);
                    continue;
                }

                if (area != data.part)
                {
                    data.Release();
                    if (data.part.empty)
                        actives.Remove(data.part);
                    data.part = area;
                    if (area.empty)
                        actives.Add(area);
                    area.AddObject(data);
                }
            }
        }

        public void ReleaseObject(T obj)
        {
            int i = 0;
            foreach(var d in objects)
            {
                if(d.obj.Equals(obj))
                {
                    d.Release();
                    objects.Release(i);
                }
                ++i;
            }
        }

        class Part
        {
            public readonly Vector3 position;
            public readonly FastList<Data> objects = new FastList<Data>();
            public bool empty { get { return objects.Count == 0; } }
            public Part(Vector3 position)
            {
                this.position = position;
            }
            public void AddObject(Data obj)
            {
                obj.index = objects.Count;
                objects.Add(obj);
            }

            public void RemoveObject(Data obj)
            {
                objects.Remove(obj.index);
                objects[obj.index].index = obj.index;

            }
        }

        class Data : ICloneable<Data>
        {
            public Vector3 position { get { return obj.position; } }
            public T obj;
            public Part part;
            public int index;

            public void Set(T obj, Part part)
            {
                this.obj = obj;
                this.part = part;
                part.AddObject(this);
            }

            public void Release()
            {
                part.RemoveObject(this);
            }

            Data ICloneable<Data>.Clone()
            {
                return new Data();
            }
            public Data()
            {
            }
        }
    }

    public interface ISpatialPartionable
    {
        Vector3 position { get; }
    }

    public class AliveShipsList : ICollection<ShipController>
    {
        List<ShipController> ships;
        public AliveShipsList(List<ShipController> ships)
        {
            this.ships = ships;
        }

        public int Count
        {
            get
            {
                return ((ICollection<ShipController>)ships).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<ShipController>)ships).IsReadOnly;
            }
        }

        public void Add(ShipController ship)
        {
            if (ship.alive)
                ships.Add(ship);
        }

        public void Clear()
        {
            ships.Clear();
        }

        public bool Contains(ShipController ship)
        {
            return ships.Contains(ship);
        }

        public void CopyTo(ShipController[] array, int arrayIndex)
        {
            ships.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ShipController> GetEnumerator()
        {
            return ships.GetEnumerator();
        }

        public bool Remove(ShipController ship)
        {
            return ships.Remove(ship);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ships.GetEnumerator();
        }
    }

    public class FriendEnemyList : ICollection<ShipController>
    {
        public readonly List<ShipController> friends;
        public readonly List<ShipController> enemies;
        public readonly ShipTeam team;
        public FriendEnemyList(ShipTeam team, List<ShipController> friends, List<ShipController> enemies)
        {
            this.team = team;
            this.friends = friends;
            this.enemies = enemies;
        }

        public int Count
        {
            get
            {
                return friends.Count + enemies.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(ShipController ship)
        {
            if (ship.alive)
                if (ship.team == team)
                    friends.Add(ship);
                else
                    enemies.Add(ship);
        }

        public void Clear()
        {
            friends.Clear();
            enemies.Clear();
        }

        public bool Contains(ShipController ship)
        {
            return friends.Contains(ship) || enemies.Contains(ship);
        }

        public void CopyTo(ShipController[] array, int arrayIndex)
        {
            friends.CopyTo(array, arrayIndex);
            enemies.CopyTo(array, arrayIndex + friends.Count);
        }

        public IEnumerator<ShipController> GetEnumerator()
        {
            foreach (var s in friends)
                yield return s;
            foreach(var s in enemies)
                yield return s;
        }

        public bool Remove(ShipController ship)
        {
            return friends.Remove(ship) || enemies.Remove(ship);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


