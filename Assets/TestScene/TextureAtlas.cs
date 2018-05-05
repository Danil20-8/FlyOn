using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextureAtlas
{
    public Texture2D atlas { get; private set; }
    Dictionary<Texture2D, TexRect> dict = new Dictionary<Texture2D, TexRect>();

    public TextureAtlas(Texture2D[] textures)
    {
        int width = textures.Select(t => t.width).Max();
        int height = textures.Select(t => t.height).Sum();
        atlas = new Texture2D(width, height);

        int yPos = 0;
        foreach(var tex in textures)
        {
            atlas.SetPixels(0, yPos, tex.width, tex.height, tex.GetPixels());

            dict.Add(tex, new TexRect() {
                leftTop = new Vector2(0, ((float)yPos) / height),
                leftBot = new Vector2(0, ((float)(yPos + tex.height)) / height),
                rightTop = new Vector2(((float)tex.width) / width, ((float)yPos) / height),
                rightBot = new Vector2(((float)tex.width) / width, ((float)(yPos + tex.height)) / height)
            });

            yPos += tex.height;
        }

        atlas.Apply();
    }
    public TexRect GetRect(Texture2D tex)
    {
        return dict[tex];
    }
}
