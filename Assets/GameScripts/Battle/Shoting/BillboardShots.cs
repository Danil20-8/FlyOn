using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Other;
using Assets.Global;
using MyLib.Algoriphms;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public abstract class BillboardShots : MonoBehaviour
{
    ArrayPool<int> arrayPool;

    int[] tris = new int[0];
    Vector3[] verts = new Vector3[0];
    Vector2[] uvs = new Vector2[0];

    int count = 0;
    SShot[] shots = new SShot[0];

    public Material material { set { GetComponent<MeshRenderer>().material = value; } }
    MeshFilter meshFilter;
    Mesh mesh;

    int size;

    SunSystemUpdater sunSystem;

    public void Begin()
    {
        sunSystem = BattleBehaviour.current.GetUpdater<MyMonoBehaviourUpdater>().GetBehaviour<SunSystemUpdater>();
    }

    void Awake()
    {
        var mr = GetComponent<MeshRenderer>();
        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;

        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        mesh.MarkDynamic();

        meshFilter.mesh = mesh;


        int[] trisSample;
        Init(out size, out trisSample);

        arrayPool = new TrisArrayPool(trisSample, size);
        arrayPool.GetArray(200);
    }
    protected abstract void Init(out int size, out int[] trisSample);
    public abstract void GetTextures(out Texture2D[] texs);
    public abstract void SetTextures(TextureAtlas atlas);

    List<int> toRemove = new List<int>();

    RaycastHit[] hits = new RaycastHit[10];
    public void UpdateMesh()
    {
        int t_count = toRemove.Count;
        int t_i;
        for(int i = 0; i < t_count; i++)
        {
            t_i = toRemove[i];

            count--;
            var t = shots[t_i];
            shots[t_i] = shots[count];
            shots[count] = t;
        }
        toRemove.Clear();

        tris = arrayPool.GetArray(count);

        camPos = Camera.main.transform.position;
        deltaTime = Time.deltaTime;

        PRun.Split(count, UpdateShots);

        mesh.vertices = verts;
        mesh.SetTriangles(tris, 0, false);
        mesh.uv = uvs;
        mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));

        for (int i = 0; i < count; i++)
        {
            if (shots[i].dead < Time.time)
                toRemove.Add(i);
            else {
                RaycastHit hit;
                if (CheckHits(Physics.RaycastNonAlloc(shots[i].back, shots[i].pos - shots[i].back, hits, shots[i].distance, Constants.shotMask), shots[i], out hit))
                {
                    shots[i].shot.Hit(hit);
                    toRemove.Add(i);
                }

            }
        }

    }
    Vector3 camPos;
    float deltaTime;
    void UpdateShots(int left, int right)
    {
        for (int i = left; i < right; i++)
        {
            shots[i].velocity += sunSystem.GetGravityAcceleration(shots[i].pos) * deltaTime;
            shots[i].speed = shots[i].velocity.magnitude;
            shots[i].distance = shots[i].speed * deltaTime;

            shots[i].back = shots[i].pos;
            shots[i].pos += shots[i].velocity * deltaTime;

            int n = i * size;

            Vector3 up = Vector3.Cross(shots[i].back - camPos, shots[i].velocity);
            up.Normalize();
            shots[i].up = up;

            UpdateShot(new ProxyArray<Vector3>(verts, n), new ProxyArray<Vector2>(uvs, n), shots[i]);
        }
    }

    bool CheckHits(int length, SShot shot, out RaycastHit hit)
    {
        RaycastHit nearesthit;
        float distance;

        RaycastHit t_hit;
        int i = 0;
        for (; i < length; i++)
        {
            t_hit = hits[i];
            if (!ShipIdentification.IsThisShip(t_hit.transform, shot.friend))
            {
                nearesthit = t_hit;
                distance = t_hit.distance;
                i++;
                goto begin;
            }
        }
        hit = new RaycastHit();
        return false;
        begin:
        float t_distance;
        for (; i < length; i++)
        {
            t_hit = hits[i];
            if (!ShipIdentification.IsThisShip(t_hit.transform, shot.friend))
            {
                t_distance = t_hit.distance;
                if (t_distance < distance)
                {
                    nearesthit = t_hit;
                    distance = t_distance;
                }
            }
        }
        hit = nearesthit;
        return true;
    }

    protected abstract void UpdateShot(ProxyArray<Vector3> verts, ProxyArray<Vector2> uvs, SShot shot);

    public void Fire(IShot shot, Vector3 start, Vector3 dir, float speed, float lifeTime, ShipController friend)
    {
        if (count == shots.Length)
        {
            int l = shots.Length;
            int wl = shots.Length * 2 + 1;
            Algs.IncreseArray(ref shots, wl);
            Algs.IncreseArray(ref verts, wl * size);
            Algs.IncreseArray(ref uvs, wl * size);

            for (int i = l; i < wl; i++)
            {
                shots[i] = new SShot();
            }
        }

        shots[count].pos = start;
        shots[count].back = shots[count].pos;
        shots[count].velocity = dir * speed;
        shots[count].dead = Time.time + lifeTime;
        shots[count].friend = friend;
        shots[count].shot = shot;
        count++;
    }

    class TrisArrayPool : PrototypeArrayPool<int>
    {
        int elementSize;
        public TrisArrayPool(int[] trisSample, int elementSize) : base(trisSample)
        {
            this.elementSize = elementSize;
        }

        protected override void HandleArray(int[] array)
        {
            base.HandleArray(array);
            int offset = 0;
            for (int i = 0; i < array.Length; i += multiply, offset += elementSize)
            {
                for (int j = 0; j < multiply; j++)
                    array[i + j] += offset;
            }
                
        }
    }
}

public class SShot
{
    public ShipController friend;
    public Vector3 pos;
    public Vector3 back;
    public Vector3 up;
    public Vector3 velocity;
    public float dead;
    public float speed;
    public float distance;
    public IShot shot;
}


public struct ProxyArray<T>
{
    public T this[int index]
    {
        set
        {
            array[index + start] = value;
        }
    }

    public ProxyArray(T[] array, int start)
    {
        this.array = array;
        this.start = start;
    }

    T[] array;
    int start;
}

class ArrayPool<T>
{
    public ArrayPool(int multiply)
    {
        this.multiply = multiply;
        this.pool = new List<T[]>();
    }

    public T[] GetArray(int size)
    {
        if (size >= pool.Count)
            Increase(size - pool.Count + 1);
        return pool[size];
    }

    void Increase(int size)
    {
        int arraySize = pool.Count * multiply;
        for (int i = 0; i < size; i++)
        {
            T[] arr = new T[arraySize];
            HandleArray(arr);
            pool.Add(arr);
            arraySize += multiply;
        }
    }

    protected virtual void HandleArray(T[] array)
    {

    }

    protected int multiply { get; private set; }
    private List<T[]> pool;
}

class PrototypeArrayPool<T> : ArrayPool<T>
{
    public PrototypeArrayPool(T[] prototype) : base(prototype.Length)
    {
        this.prototype = prototype;
    }

    protected override void HandleArray(T[] array)
    {
        for (int i = 0; i < array.Length / multiply; i++)
            Array.Copy(prototype, 0, array, i * multiply, multiply);
    }
    T[] prototype;
}