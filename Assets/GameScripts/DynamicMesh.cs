using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using System.Collections;
using MyLib.Algoriphms;

public class DynamicMesh
{
    public int elementsCount
    {
        get { return _elementsCount; }
        set
        {
            if (value > _maxElementsCount)
            {
                int maxElementsCount = (_maxElementsCount + (value - _maxElementsCount)) * 2;

                Algs.IncreseArray(ref verts, maxElementsCount * elementSize);
                if (vertsSample != null)
                    for (int i = _maxElementsCount; i < maxElementsCount; i++)
                        Array.Copy(verts, i * elementSize, vertsSample, 0, elementSize);

                Algs.IncreseArray(ref uvs, maxElementsCount * elementSize);
                if (uvsSample != null)
                    for (int i = _maxElementsCount; i < maxElementsCount; i++)
                        Array.Copy(uvs, i * elementSize, uvsSample, 0, elementSize);

                Algs.IncreseArray(ref colors, maxElementsCount * elementSize);
                if(colorsSample != null)
                    for (int i = _maxElementsCount; i < maxElementsCount; i++)
                        Array.Copy(colors, i * elementSize, colorsSample, 0, elementSize);

                _maxElementsCount = maxElementsCount;
            }
            if (value != _elementsCount)
            {
                tris = new int[value * elementSize * 3];
                if(trisSample != null)
                    for (int i = 0; i < _maxElementsCount; i++)
                        Array.Copy(uvs, i * trisSize, trisSample, 0, trisSize);
            }

            _elementsCount = value;
        }
    }
    int _elementsCount = 0;
    int _maxElementsCount = 0;


    int[] trisSample;
    Vector2[] uvsSample;
    Color[] colorsSample;
    Vector3[] vertsSample;


    public readonly int elementSize;
    readonly int trisSize;
    Mesh mesh;

    Vector3[] verts = new Vector3[0];
    int[] tris = new int[0];
    Vector2[] uvs = new Vector2[0];
    Color[] colors = new Color[0];


    public DynamicMesh(int elementSize, int[] trisSample = null, Vector2[] uvsSample = null, Color[] colorsSample = null, Vector3[] vertsSample = null)
    {
        this.mesh = new Mesh();
        this.mesh.MarkDynamic();

        this.trisSample = trisSample;
        this.uvsSample = uvsSample;
        this.colorsSample = colorsSample;
        this.vertsSample = vertsSample;

        this.elementSize = elementSize;
        trisSize = elementSize * 3 / 2;
    }

    public DynamicMesh(Mesh meshSample)
    {
        this.mesh = new Mesh();
        this.mesh.MarkDynamic();

        this.trisSample = meshSample.triangles;
        this.uvsSample = meshSample.uv;
        this.colorsSample = meshSample.colors;
        this.vertsSample = meshSample.vertices;

        this.elementSize = vertsSample.Length;
        trisSize = trisSample.Length;
    }

    ProxyArray<Vector3> GetVerts(int elementIndex)
    {
        return new ProxyArray<Vector3>(verts, elementIndex * elementSize);
    }
    ProxyArray<int> GetTris(int elementIndex)
    {
        return new ProxyArray<int>(tris, elementIndex * trisSize);
    }
    ProxyArray<Vector2> GetUVs(int elementIndex)
    {
        return new ProxyArray<Vector2>(uvs, elementIndex * elementSize);
    }
    ProxyArray<Color> GetColors(int elementIndex)
    {
        return new ProxyArray<Color>(colors, elementIndex * elementSize);
    }
    void ResetVerts(int elementIndex)
    {
        if (vertsSample != null)
            Array.Copy(verts, elementIndex * elementSize, vertsSample, 0, elementSize);
    }
    void ResetUVs(int elementIndex)
    {
        if(uvsSample != null)
            Array.Copy(uvs, elementIndex * elementSize, uvsSample, 0, elementSize);
    }
    void ResetColors(int elementIndex)
    {
        if (colorsSample != null)
            Array.Copy(colors, elementIndex * elementSize, colorsSample, 0, elementSize);
    }
    void ResetTris(int elementIndex)
    {
        if (trisSample != null)
            Array.Copy(tris, elementIndex * trisSize, trisSample, 0, trisSize);
    }

    public IEnumerable<Element> ByElements(int elementsCount)
    {
        this.elementsCount = elementsCount;
        return new ElementSequence(this, 0, elementsCount);
    }
    void Apply()
    {
        mesh.vertices = verts;
        mesh.uv = uvs;

        int elementIndex = 0;
        for (int i = 0; i < elementsCount * trisSize; i += trisSize, elementIndex += elementSize)
        {
            int bound = i + trisSize;
            for (int j = i; j < bound; j += 3)
            {
                tris[j] += elementIndex;
                tris[j + 1] += elementIndex;
                tris[j + 2] += elementIndex;
            }
        }
        mesh.triangles = tris;

        ;
    }

    public static implicit operator Mesh(DynamicMesh mesh)
    {
        return mesh.mesh;
    }


    public static DynamicMesh CreateMultiLineMesh()
    {
        return new DynamicMesh(4, 
            new int[] { 0, 2, 1, 2, 3, 1 },
            new Vector2[] { Vector2.zero, new Vector2(1 ,0), new Vector2(0, 1), Vector2.one },
            new Color[] { Color.white, Color.white, Color.white, Color.white },
            null
            );
    }

    public struct ElementSequence : IEnumerable<Element>
    {
        DynamicMesh mesh;
        int start;
        int end;

        public ElementSequence(DynamicMesh mesh, int start, int end)
        {
            this.mesh = mesh;
            this.start = start;
            this.end = end;
        }

        public IEnumerator<Element> GetEnumerator()
        {
            for (int i = start; i < end; i++)
                yield return new Element(mesh, i);
            mesh.Apply();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public struct Element
    {
        public ProxyArray<Vector3> verts { get { return mesh.GetVerts(elementIndex); } }
        public ProxyArray<Vector2> uvs { get { return mesh.GetUVs(elementIndex); } }
        public ProxyArray<int> tris { get { return mesh.GetTris(elementIndex); } }
        public ProxyArray<Color> colors { get { return mesh.GetColors(elementIndex); } }
        DynamicMesh mesh;
        int elementIndex;

        public void ResetVerts() { mesh.ResetVerts(elementIndex); }
        public void ResetUVs() { mesh.ResetUVs(elementIndex); }
        public void ResetColors() { mesh.ResetColors(elementIndex); }
        public void ResetTris() { mesh.ResetTris(elementIndex); }

        public Element(DynamicMesh mesh, int elementIndex)
        {
            this.mesh = mesh;
            this.elementIndex = elementIndex;
        }

        public void SetLine(Vector3 origin, Vector3 end, float width, Vector3 viewPos)
        {
            Vector3 up = Vector3.Cross(origin - viewPos, end - origin);
            up.Normalize();
            up *= width;

            SetQuad(origin, end, up);
        }
        public void SetQuad(Vector3 origin, Vector3 end, Vector3 up)
        {
            var verts = this.verts;
            verts[0] = origin - up;
            verts[1] = end - up;
            verts[2] = origin + up;
            verts[3] = end + up;

            var tris = this.tris;
            tris[0] = 0;
            tris[1] = 2;
            tris[2] = 1;

            tris[3] = 2;
            tris[4] = 3;
            tris[5] = 1;

            var uvs = this.uvs;
            uvs[0] = Vector2.zero;
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 1);
            uvs[3] = Vector2.one;
        }
    }
}

