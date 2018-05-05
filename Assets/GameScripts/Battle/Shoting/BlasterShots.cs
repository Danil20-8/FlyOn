using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlasterShots : BillboardShots
{
    [SerializeField]
    Texture2D tex;

    TexRect uvRect;

    protected override void Init(out int size, out int[] trisSample)
    {
        size = 4;
        trisSample = new int[] { 0, 2, 1, 2, 3, 1 };
    }
    public override void GetTextures(out Texture2D[] texs)
    {
        texs = new Texture2D[] { tex };
    }
    public override void SetTextures(TextureAtlas atlas)
    {
        uvRect = atlas.GetRect(tex);
    }

    protected override void UpdateShot(ProxyArray<Vector3> verts, ProxyArray<Vector2> uvs, SShot shot)
    {
        Vector3 up = shot.up * .75f;

        verts[0] = shot.back - up;
        verts[1] = shot.pos - up;
        verts[2] = shot.back + up;
        verts[3] = shot.pos + up;


        uvs[0] = uvRect.leftBot;
        uvs[1] = uvRect.rightBot;
        uvs[2] = uvRect.leftTop;
        uvs[3] = uvRect.rightTop;

        /*
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;

        tris[3] = 2;
        tris[4] = 3;
        tris[5] = 1;*/
    }
}

