using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib.Algoriphms;

public class MultiLineBehaviour : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    float width;

    DynamicMesh mesh;

    void Awake()
    {
        gameObject.AddComponent<MeshRenderer>().material = material;

        mesh = new DynamicMesh(4);

        gameObject.AddComponent<MeshFilter>().mesh = mesh;

    }

    List<Line> lines = new List<Line>();
    public void AddLine(Vector3 origin, Vector3 end)
    {
        lines.Add(new Line { origin = origin, end = end });
    }

    public void Show()
    {
        Vector3 camera = Camera.main.transform.position;
        var line = lines.GetEnumerator();

        foreach(var e in mesh.ByElements(lines.Count))
        {
            line.MoveNext();
            var l = line.Current;
            e.SetLine(l.origin, l.end, width, camera);
        }

        line.Dispose();
        lines.Clear();
    }
    public void ShowLocalSpace()
    {
        Vector3 camera = transform.worldToLocalMatrix.MultiplyPoint3x4(Camera.allCameras[0].transform.position);

        var line = lines.GetEnumerator();

        Line l;
        foreach (var e in mesh.ByElements(lines.Count))
        {
            line.MoveNext();
            l = line.Current;
            e.SetLine(l.origin, l.end, width,  camera);
        }

        line.Dispose();
        lines.Clear();
    }
    struct Line
    {
        public Vector3 origin;
        public Vector3 end;
    }
}

