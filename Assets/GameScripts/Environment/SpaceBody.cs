using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;

public class SpaceBody: MyMonoBehaviour
{
    public float radius { get { return _radius; } set { _radius = value; _mass = Constants.worldGravity * value; } }
    float _radius;
    public float mass { get { return _mass; } }
    float _mass;

    public TempTransform tempTransform;

    protected void Awake()
    {
        tempTransform = AddMyComponent<TempTransform>();

        OnAwake();
    }
    protected virtual void OnAwake()
    {

    }
}
