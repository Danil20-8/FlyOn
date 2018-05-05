using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SatelliteBehaviour : PBehaviour
{
    Transform transform;
    Transform sun;
    Vector3 axis;
    float speed;

    public override void Start()
    {
        transform = behaviour.transform;
        var rb = behaviour.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

    }

    public void SetSun(Transform sun, float radius, Vector3 axis)
    {
        this.sun = sun;
        this.axis = axis;
        speed = radius / Vector3.Distance(behaviour.transform.position, sun.position);
    }

    public override void FastUpdate()
    {

    }

    public override void InitializeUpdate()
    {
    }

    public override void SlowUpdate()
    {

        transform.RotateAround(sun.position, axis, speed * Time.deltaTime);
        transform.Rotate(axis, .05f);
    }
}

