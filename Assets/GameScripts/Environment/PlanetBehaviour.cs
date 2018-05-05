using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyLib.Algoriphms;
using Assets.Other;

public class PlanetBehaviour: SpaceBody
{
    const int texture_width = 256;
    const int texture_height = 128;

    protected override void OnAwake()
    {
        tempTransform.enabled = false;
    }

    protected override void InitializeUpdate()
    {
        tempTransform.position = transform.position;
    }

    public void Initialize(SunBehaviour sun, Vector3 axis, float radius, AsyncProcessor asyncProcessor)
    {
        if (asyncProcessor != null)
            asyncProcessor.Process(() => AsyncInitialize(sun, axis, radius));
        else
            Initialize(sun, axis, radius);
    }
    IEnumerator AsyncInitialize(SunBehaviour sun, Vector3 axis, float radius)
    {
        InitializePlanet(sun, axis, radius);
        Color[] texture = null;
        yield return new AsyncProcess(() => {
            texture = GenerateTexture(texture_width, texture_height);
        }
        ).Process();
        SetTexture(texture, texture_width, texture_height);
    }
    public void Initialize(SunBehaviour sun, Vector3 axis, float radius)
    {
        InitializePlanet(sun, axis, radius);
        Color[] texture = GenerateTexture(texture_width, texture_height);
        SetTexture(texture, texture_width, texture_height);
    }
    void InitializePlanet(SunBehaviour sun, Vector3 axis, float radius)
    {
        transform.localScale = Vector3.one * radius;
        this.radius = radius;

        AddMyComponent<SatelliteBehaviour>()
            .SetSun(sun.transform, sun.radius, axis);

        ships = new ShipList(this, axis, .05f);
    }
    Color[] GenerateTexture(int width, int height)
    {
        var random = ConcurrentRandomProvider.GetRandom();

        Color[] usecs = Enumerable.Range(0, random.Next(2, 5)).Select(i => new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble())).ToArray();

        return Algs.GenerateColorArray(usecs, width, height, 2, random);
    }

    void SetTexture(Color[] colors, int width, int height)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(colors);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    void InitializeMesh()
    {
        var vs = GetComponent<MeshFilter>().mesh.vertices;
        foreach (var v in vs.ToLookup(v => v, v => vs.IndexOf(v)))
        {
            var r = v.Key + v.Key.normalized * Random.value * .025f;
            foreach (var g in v)
                vs[g] = r;
        }

        GetComponent<MeshFilter>().mesh.vertices = vs;
        GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }
    public void InitializeAtmosphere(Color sunColor, Vector3 sunPosition, float sunRadius, float planetRadius, AsyncProcessor asyncProcessor)
    {
        var at = Resources.Load<AtmosphereBehaviour>(@"Environment\Atmosphere");
        var a = Instantiate(at, transform.position, transform.rotation);
        a.transform.SetParent(transform);

        a.Initialize(sunColor, sunPosition, sunRadius, planetRadius, asyncProcessor);
    }

    ShipList ships;

    protected override void FastUpdate()
    {
        BattleBehaviour.current.area.GetObjects(tempTransform.position, radius, ships);
    }

    protected override void SlowUpdate()
    {
        ships.ApplyRotation();
    }

    class ShipList : DoubleList<ShipController, float>
    {
        PlanetBehaviour planet;

        Vector3 axis;
        float speed;

        float sqrRadius;
        public ShipList(PlanetBehaviour planet, Vector3 axis, float speed)
        {
            this.planet = planet;
            sqrRadius = planet.radius * planet.radius;

            this.axis = axis;
            this.speed = speed;
        }

        public override void Add(ShipController ship)
        {
            float sqrDistance = (planet.tempTransform.position - ship.tempTransform.position).sqrMagnitude;
            float t = Mathf.Sqrt(sqrRadius / sqrDistance);
            Add(ship, t);
        }

        public void ApplyRotation()
        {
            float angle = speed * BattleBehaviour.deltaTime;
            foreach (var e in this)
                e.Item1.transform.RotateAround(planet.tempTransform.position, axis, angle * e.Item2);
        }
    }
}