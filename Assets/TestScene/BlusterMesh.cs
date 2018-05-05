using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Other;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BlusterMesh: MonoBehaviour
{
    [SerializeField]
    Texture2D blusterTex;
    [SerializeField]
    Texture2D rocketTex;

    int[] tris = new int[0];
    Vector3[] verts = new Vector3[0];
    Vector2[] uvs = new Vector2[0];

    int count = 0;
    Shot[] shots = new Shot[0];

    List<int> toRemove = new List<int>();

    new Transform camera;

    MeshFilter meshFilter;
    Mesh mesh;

    Texture2D texAtlas;
    TexRect[] atlasMap;
    
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        

        mesh.MarkDynamic();

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));

        meshFilter.mesh = mesh;

        camera = Camera.main.transform;

        int width = Mathf.Max(blusterTex.width, rocketTex.width);
        texAtlas = new Texture2D(width, blusterTex.height + rocketTex.height);
        texAtlas.SetPixels(0, 0, width, blusterTex.height, blusterTex.GetPixels());
        texAtlas.SetPixels(0, blusterTex.height, width, rocketTex.height, rocketTex.GetPixels());
        texAtlas.Apply();

        atlasMap = new TexRect[2];
        atlasMap[0] = new TexRect();
        atlasMap[1] = new TexRect();

        atlasMap[0].leftTop = Vector2.zero;
        atlasMap[0].leftBot = new Vector2(0, ((float)blusterTex.height) / texAtlas.height);
        atlasMap[0].rightTop = new Vector2(((float)blusterTex.width) / texAtlas.width, 0);
        atlasMap[0].rightBot = new Vector2(((float)blusterTex.width) / texAtlas.width, ((float)blusterTex.height) / texAtlas.height);

        atlasMap[1].leftTop = new Vector2(0, ((float)blusterTex.height) / texAtlas.height);
        atlasMap[1].leftBot = new Vector2(0, ((float)(blusterTex.height + rocketTex.height)) / texAtlas.height);
        atlasMap[1].rightTop = new Vector2(((float)rocketTex.width) / texAtlas.width, ((float)blusterTex.height) / texAtlas.height);
        atlasMap[1].rightBot = new Vector2(((float)rocketTex.width) / texAtlas.width, ((float)(blusterTex.height + rocketTex.height)) / texAtlas.height);

        GetComponent<MeshRenderer>().material.mainTexture = texAtlas;
    }

    public void UpdateMesh()
    {
        foreach(var i in toRemove)
        {
            count--;
            var t = shots[i];
            shots[i] = shots[count];
            shots[count] = t;
        }
        toRemove.Clear();

        int ntris = count * 6;
        if (tris.Length != ntris)
            tris = new int[ntris];

        Vector3 camPos = camera.position;

        //PRun.For(count, i =>
        PRun.Split(count, (left, right) =>
        {
            for(int i = left; i < right; i++)
            {
                int n = i * 4;
                int n1 = n + 1;
                int n2 = n + 2;
                int n3 = n + 3;

                Vector3 up = Vector3.Cross(shots[i].back - camPos, shots[i].dir);
                up.Normalize();
                up *= shots[i].width;

                switch (shots[i].type)
                {
                    case SizeType.Dynamic:
                        verts[n] = shots[i].back - up;
                        verts[n1] = shots[i].pos - up;
                        verts[n2] = shots[i].back + up;
                        verts[n3] = shots[i].pos + up;
                        break;
                    case SizeType.Static:
                        var back = shots[i].pos - shots[i].dir * shots[i].length;

                        verts[n] = back - up;
                        verts[n1] = shots[i].pos - up;
                        verts[n2] = back + up;
                        verts[n3] = shots[i].pos + up;
                        break;
                }

                int nuv = i * 4;
                uvs[nuv] = shots[i].uvRect.leftBot;
                uvs[nuv + 1] = shots[i].uvRect.rightBot;
                uvs[nuv + 2] = shots[i].uvRect.leftTop;
                uvs[nuv + 3] = shots[i].uvRect.rightTop;


                int nt = i * 6;

                tris[nt] = n;
                tris[nt + 1] = n2;
                tris[nt + 2] = n1;

                tris[nt + 3] = n2;
                tris[nt + 4] = n3;
                tris[nt + 5] = n1;
            }
        });

        mesh.vertices = verts;
        mesh.SetTriangles(tris, 0, false);
        //mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));

        for (int i = 0; i < count; i++)
        {
            if (shots[i].dead < Time.time)
                toRemove.Add(i);
            else {
                shots[i].back = shots[i].pos;
                float distance = shots[i].speed * Time.deltaTime;
                shots[i].pos += shots[i].dir * distance;

                RaycastHit hit;
                if (Physics.Raycast(shots[i].back, shots[i].pos - shots[i].back, out hit, distance))
                {
                    if (hit.transform.IsOne(shots[i].friend))
                        continue;
                    shots[i].shot.Hit(hit);
                    toRemove.Add(i);
                }

            }
        }

    }

    void IncreseArray<T>(ref T[] arr, int new_size)
    {
        T[] t = new T[new_size];
        for (int i = 0; i < arr.Length; i++)
            t[i] = arr[i];
        arr = t;
    }

    public void Fire(ShotType type, IShot shot, Vector3 start, Vector3 dir, float speed, float lifeTime, Transform friend)
    {
        if (count == shots.Length)
        {
            int l = shots.Length;
            int wl = shots.Length * 2 + 1;
            IncreseArray(ref shots, wl);
            IncreseArray(ref verts, wl * 4);
            IncreseArray(ref uvs, wl * 4);

            for (int i = l; i < wl; i++)
            {
                shots[i] = new Shot();
            }
        }
        switch(type)
        {
            case ShotType.Bluster:
                shots[count].pos = start;
                shots[count].type = SizeType.Dynamic;
                shots[count].uvRect = atlasMap[0];
                shots[count].width = .75f;
                break;
            case ShotType.Rocket:
                shots[count].pos = start + dir * 12f;
                shots[count].type = SizeType.Static;
                shots[count].length = 6f;
                shots[count].width = 1f;
                shots[count].uvRect = atlasMap[1];
                break;
        }
        shots[count].back = shots[count].pos;
        shots[count].dir = dir;
        shots[count].speed = speed;
        shots[count].dead = Time.time + lifeTime;
        shots[count].shot = shot;
        shots[count].friend = friend;

        count++;
    }
}

class Shot
{
    public Transform friend;
    public Vector3 pos;
    public Vector3 back;
    public Vector3 dir;
    public float speed;
    public float dead;
    public float width;
    public float length;

    public SizeType type;
    public IShot shot;

    public TexRect uvRect;
}

enum SizeType: byte
{
    Dynamic,
    Static
}

public enum ShotType
{
    Bluster,
    Rocket
}


public class TexRect
{
    public Vector2 leftBot;
    public Vector2 leftTop;
    public Vector2 rightBot;
    public Vector2 rightTop;
}

public interface IShot
{
    void Hit(RaycastHit hit);
}