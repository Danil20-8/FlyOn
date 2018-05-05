using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShotSystem: MonoBehaviour
{
    [SerializeField]
    BillboardShots[] shots;

    [SerializeField]
    Shader sh;

    void Start()
    {
        List<Texture2D> texs = new List<Texture2D>();

        foreach(var s in shots)
        {
            Texture2D[] ts;
            s.GetTextures(out ts);
            texs.AddRange(ts);
        }

        Material mat = new Material(sh);
        TextureAtlas ta = new TextureAtlas(texs.ToArray());
        mat.mainTexture = ta.atlas;

        foreach (var s in shots)
        {
            s.material = mat;
            s.SetTextures(ta);
        }

        BattleBehaviour.AddEvent(() => { foreach (var s in shots) s.Begin(); });
    }

    public void UpdateShots()
    {
        foreach (var s in shots)
            s.UpdateMesh();
    }

    public T GetSystem<T>() where T : BillboardShots
    {
        foreach (var s in shots)
            if (s is T)
                return (T)s;
        throw new Exception("System's not found");
    }
}

