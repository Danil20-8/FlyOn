using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System.Collections;
using Assets.Other;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(Renderer))]
public class SunBehaviour: SpaceBody
{
    const int animation_frames = 60;
    const int texture_size = 256;

    protected override void OnAwake()
    {
        tempTransform.enabled = false;
        tempTransform.position = transform.position;
    }

    public void SetSun(Color c, float radius)
    {
        transform.localScale = Vector3.one * radius;
        var l = GetComponent<Light>();
        l.color = Color.Lerp(c, Color.white, .2f);
        l.range = radius ;
        l.type = LightType.Point;

        int size = 128;
        Color32[] cs = new Color32[size * size];
        Color[] usecs = new Color[] { c * .8f, c, c * (2f * .8f) };
        var t1 = GenerateTexture(usecs, size);
        var t2 = GenerateTexture(usecs, size);
        gameObject.AddComponent<AnimationTextureBehaviour>()
            .SetTexture(Enumerable.Range(0, 30).Select(i => LerpTexture(t1, t2, 1f / 30 * i)).ToArray(), 1f, true);
    }

    public void Initialize(Color color, float radius, AsyncProcessor asyncProcessor)
    {
        if (asyncProcessor != null)
            asyncProcessor.Process(() => AsyncInitialize(color, radius));
        else
            Initialize(color, radius);
    }
    public void Initialize(Color color, float radius)
    {
        Init(color, radius);

        int size = texture_size;

        Color[][] animation = GenerateAnimation(color, size);
        SetAnimation(animation, size);
    }
    IEnumerator AsyncInitialize(Color color, float radius)
    {
        Init(color, radius);

        int size = texture_size;

        Color[][] animation = null;

        yield return new AsyncProcess(() =>
        {
            animation = GenerateAnimation(color, size);
        }).Process();

        SetAnimation(animation, size);
    }
    void Init(Color color, float radius)
    {
        transform.localScale = Vector3.one * radius;
        var l = GetComponent<Light>();
        l.color = color * 1.1f;
        l.range = radius * radius;
        l.type = LightType.Point;
    }
    Color[][] GenerateAnimation(Color color, int size)
    {
        Color[] usecs = new Color[] { color * .8f, color, color * (2f * .8f) };

        Color[][] animation = new Color[animation_frames][];

        var random = ConcurrentRandomProvider.GetRandom();

        Color[] origin = Algs.GenerateColorArray(usecs, size, size, 1, random);
        Color[] final = Algs.GenerateColorArray(usecs, size, size, 1, random);

        for (int i = 1; i < animation_frames - 1; i++)
            animation[i] = LerpColorArray(origin, final, ((float)i) / animation_frames);

        animation[0] = origin;
        animation[animation_frames - 1] = final;

        return animation;
    }
    void SetAnimation(Color[][] animation, int size)
    {
        gameObject.AddComponent<AnimationTextureBehaviour>()
            .SetTexture(animation.Select(a =>
            {
                Texture2D tex = new Texture2D(size, size);
                tex.SetPixels(a);
                tex.Apply();
                return tex;
            }).ToArray(), 1f, true);
    }
    Texture2D GenerateTexture(Color[] colors, int size)
    {
        Texture2D tex = new Texture2D(size, size);

        var random = ConcurrentRandomProvider.GetRandom();

        tex.SetPixels(Algs.GenerateColorArray(colors, size, size, 1, random));
        tex.Apply();
        return tex;
    }

    Texture2D LerpTexture(Texture2D origin, Texture2D end, float t)
    {
        int width = origin.width;
        int height = origin.height;
        Texture2D r = new Texture2D(width, height);
        r.SetPixels(origin.GetPixels().Select(end.GetPixels(), (o, e) => Color.Lerp(o, e, t)).ToArray());
        r.Apply();
        return r;
    }
    Color[] LerpColorArray(Color[] origin, Color[] final, float t)
    {
        Color[] result = new Color[origin.Length];
        for (int i = 0; i < origin.Length; i++)
            result[i] = Color.Lerp(origin[i], final[i], t);
        return result;
    }
}
