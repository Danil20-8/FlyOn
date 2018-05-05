using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TempTransform: PBehaviour
{
    Transform transform;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 forward;
    public Vector3 up;
    public Vector3 right;

    public override void Start()
    {
        transform = behaviour.transform;
    }

    public override void InitializeUpdate()
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.forward = transform.forward;
        this.up = transform.up;
        this.right = transform.right;
    }

    public override void FastUpdate()
    {
    }

    public override void SlowUpdate()
    {
    }
}

