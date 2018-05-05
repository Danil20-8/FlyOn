using System.Linq;
using System.Collections;
using UnityEngine;
using Assets.Other;

public class AtmosphereBehaviour : MonoBehaviour
{
    [SerializeField]
    Shader atmosphereShader;

    Quaternion rot = Quaternion.identity;

    public void Initialize(Color sunColor, Vector3 sunPosition, float sunRadius, float planetRadius, AsyncProcessor asyncProcessor)
    {
        if (asyncProcessor != null)
            asyncProcessor.Process(() => InitializeAsync(sunPosition, sunColor, sunRadius, planetRadius));
        else
            Initialize(sunColor, sunPosition, sunRadius, planetRadius, new System.Random());
    }

    IEnumerator InitializeAsync(Vector3 sunPosition, Color sunColor, float sunRadius, float planetRadius)
    {
        Color[] texArray = null;
        var random = ConcurrentRandomProvider.GetRandom();
        yield return new AsyncProcess(() =>
        {
            texArray = GenerateColorArray(random);
        }).Process();

        InitializeAtmosphere(texArray, sunPosition, sunColor, sunRadius, planetRadius);
    }

    public void Initialize(Color sunColor, Vector3 sunPosition, float sunRadius, float planetRadius, System.Random random)
    {
        InitializeAtmosphere(GenerateColorArray(random), sunPosition, sunColor, sunRadius, planetRadius);
    }

    const int size = 64;
    Color[] GenerateColorArray(System.Random random)
    {
        Color[] usecs = Enumerable.Range(0, random.Next(2, 5)).Select(i => RandomTColor(random)).ToArray();
        return Algs.GenerateColorArray(usecs, size, size, 1, random);
    }
    void InitializeAtmosphere(Color[] texArray, Vector3 sunPosition, Color sunColor, float sunRadius, float planetRadius)
    {

        Texture2D tex = new Texture2D(size, size);
        tex.SetPixels(texArray);
        tex.Apply();

        Material mat = new Material(atmosphereShader);
        mat.SetTexture("_MainTex", tex);
        mat.SetVector("_SunPosition", sunPosition);
        mat.SetColor("_SunColor", sunColor);
        GetComponent<Renderer>().material = mat;

        float distance = Vector3.Distance(transform.position, sunPosition);
        float rSub = sunRadius - planetRadius;
        float rimDistance = Mathf.Sqrt(distance * distance + rSub * rSub);
        mat.SetFloat("_SunDot", rSub / rimDistance);

        transform.localScale = Vector3.one * 1.25f;
    }

    Color RandomTColor(System.Random random)
    {
        System.Func<byte> rb = () => (byte)(random.Next(255));
        return new Color32(rb(), rb(), rb(), (byte)(50 + random.Next(150)));
    }

    void Update()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rot, 5 * Time.deltaTime);
        if (Quaternion.Angle(transform.localRotation, rot) == 0)
        {
            rot = Random.rotation;
        }
    }
}

