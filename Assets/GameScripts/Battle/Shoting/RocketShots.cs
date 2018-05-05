using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RocketShots : BillboardShots
{
    [SerializeField]
    Texture2D rocketTexture;
    [SerializeField]
    Texture2D trailTexture;

    TexRect uvRocket;
    TexRect uvTrail;

    float length = 8;
    float width = 1;

    public override void GetTextures(out Texture2D[] texs)
    {
        texs = new Texture2D[] { rocketTexture, trailTexture};
    }

    public override void SetTextures(TextureAtlas atlas)
    {
        uvRocket = atlas.GetRect(rocketTexture);
        uvTrail = atlas.GetRect(trailTexture);
    }

    protected override void Init(out int size, out int[] trisSample)
    {
        size = 8;
        trisSample = new int[] {
            0, 2, 1, 2, 3 , 1,
            4, 6, 5, 6, 7 ,5
        };
    }

    protected override void UpdateShot(ProxyArray<Vector3> verts, ProxyArray<Vector2> uvs, SShot shot)
    {
        Vector3 up = shot.up * width;
        Vector3 rocketBack = shot.pos - shot.velocity / (shot.speed / length);
        //rocket
        verts[0] = rocketBack - up;
        verts[1] = shot.pos - up;
        verts[2] = rocketBack + up;
        verts[3] = shot.pos + up;


        uvs[0] = uvRocket.leftBot;
        uvs[1] = uvRocket.rightBot;
        uvs[2] = uvRocket.leftTop;
        uvs[3] = uvRocket.rightTop;

        /*
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;

        tris[3] = 2;
        tris[4] = 3;
        tris[5] = 1;*/

        //trail

        verts[4] = shot.back - up;
        verts[5] = rocketBack - up;
        verts[6] = shot.back + up;
        verts[7] = rocketBack + up;


        uvs[4] = uvTrail.leftBot;
        uvs[5] = uvTrail.rightBot;
        uvs[6] = uvTrail.leftTop;
        uvs[7] = uvTrail.rightTop;

        /*
        tris[6] = 4;
        tris[7] = 6;
        tris[8] = 5;

        tris[9] = 6;
        tris[10] = 7;
        tris[11] = 5;*/
    }
}

