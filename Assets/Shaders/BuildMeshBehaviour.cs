using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using MyLib.Algoriphms;

public class BuildMeshBehaviour : MonoBehaviour {

    [SerializeField]
    MultiLineBehaviour lines;

    [SerializeField]
    Material buildMaterial;

    [SerializeField]
    new MeshRenderer renderer;

    Transform rTransform;
    Material originMaterial;
    Mesh mesh;

    const float time_to_shader_time = 1f / 20f;

    public void SetBuildObject(MeshRenderer renderer, float duration, Action callback)
    {
        this.renderer = renderer;
        this.mesh = renderer.GetComponent<MeshFilter>().mesh;
        rTransform = renderer.transform;

        originMaterial = renderer.material;

        Material bm = (renderer.material = buildMaterial);

        bm.SetTexture("_MainTex", originMaterial.mainTexture);
        bm.SetFloat("_StartTime", Time.time);
        bm.SetFloat("_Duration", duration * time_to_shader_time);



        StartCoroutine(Wait(duration, callback));
    }

    IEnumerator Wait(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);

        callback();

        renderer.material = originMaterial;
        enabled = false;
    }


	void Start () {

        ls = new List<Line>(Enumerable.Range(0, 5).Select(i => new Line()));
        
        SetBuildObject(renderer, 20, () => {; });
	}

    List<Line> ls;
    float t = 1f;
    float speed;
	void Update () {
	    if(t >= 1f)
        {
            foreach(var l in ls)
            {
                l.begin = l.end;
                l.end = mesh.vertices[UnityEngine.Random.Range(0, mesh.vertices.Length)];
            }
            t = 0;
        }
        t += Time.deltaTime;
        foreach(var l in ls)
            lines.AddLine(Vector3.zero, Vector3.Lerp(l.begin, l.end, t));

        lines.ShowLocalSpace();
	}

    class Line
    {
        public Vector3 begin = Vector3.zero;
        public Vector3 end = Vector3.zero;
    }
}
