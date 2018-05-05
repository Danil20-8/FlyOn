using UnityEngine;
using System.Collections.Generic;
using Assets.Global;
using Assets.Other;
using MyLib.Modern;
using System;

public class WorldlGravity : PBehaviour {

    TempTransform transform;
    float radius;
    float force;
    float maxDistance;

    GravityList ships;

	// Use this for initialization
	public override void Start () {
        transform = GetComponent<TempTransform>();

        radius = behaviour.transform.localScale.x;
        force = Constants.worldGravity * radius;
        maxDistance = force / .1f;

        ships = new GravityList(this);
	}
	
	// Update is called once per frame
	public override void FastUpdate () {
        BattleBehaviour.current.area.GetObjects(transform.position, maxDistance, ships);
	}

    public override void SlowUpdate()
    {
        ships.ApplyGravity();
    }

    public override void InitializeUpdate()
    {
    }

    class GravityList : DoubleList<ShipController, Vector3>
    {
        WorldlGravity wg;
        public GravityList(WorldlGravity wg)
        {
            this.wg = wg;
        }

        public override void Add(ShipController ship)
        {
            var v = wg.transform.position - ship.tempTransform.position;
            var d = v.sqrMagnitude;
            Add(ship, v * (wg.force / d * BattleBehaviour.deltaTime));
        }

        public void ApplyGravity()
        {
            foreach (var e in this)
                e.Item1.AddForce(e.Item2);
        }
    }
}
