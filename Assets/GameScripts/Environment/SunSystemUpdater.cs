using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using MyLib.Algoriphms;
public class SunSystemUpdater : IMyMonoBehaviour
{
    public SunSystemUpdater(SunBehaviour sun, PlanetBehaviour[] planets)
    {
        this.bodies = new SpaceBody[] { sun }.Concat(planets).ToArray();
    }
    public SunSystemUpdater(IEnumerable<SpaceBody> bodies)
    {
        this.bodies = bodies.ToArray();
    }
    public Vector3 GetGravityAcceleration(Vector3 point)
    {
        Vector3 r = Vector3.zero;
        foreach (var b in bodies) {
            var dir = b.tempTransform.position - point;
            var sqrDist = dir.sqrMagnitude;
            r += dir * (b.mass / sqrDist);
                }
        return r;
    }

    public void __InitializeUpdate()
    {
        foreach (var b in bodies)
            b.__InitializeUpdate();
    }

    public void __FastUpdate()
    {
        foreach (var b in bodies)
            b.__FastUpdate();
    }

    public void __SlowUpdate()
    {
        foreach (var b in bodies)
            b.__SlowUpdate();
    }

    private SpaceBody[] bodies;
}

