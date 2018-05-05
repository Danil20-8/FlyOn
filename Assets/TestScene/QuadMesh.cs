using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class QuadMesh : MonoBehaviour {


    float height = 5;
    float width = 5;
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        var mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        vertices[0] = Vector3.up + new Vector3(0, 0, 0);
        vertices[1] = new Vector3(width, 0, 0);
        vertices[2] = new Vector3(0, height, 0);
        vertices[3] = new Vector3(width, height, 0);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[4];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4];

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;

    }
}
