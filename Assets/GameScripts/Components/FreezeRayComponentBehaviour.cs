using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
using Assets.GameModels.ShipEffects;
using Assets.GameModels;
using Assets.GameModels.Components;
using Assets.Global;
using MyLib.Algoriphms;

[RequireComponent(typeof(LineRenderer))]
class FreezeRayComponentBehaviour : GunComponentBehaviour
{

    [SerializeField]
    Transform firePoint;
    Vector3 shotPoint;
    LineRenderer lineRenderer;
    float sqrMaxDistance;

    bool bFiring = false;
    float distance;

    protected override void OnStart()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
    }

    public void Fire(float distance)
    {
        this.distance = distance;
        bfire = true;
    }

    protected override void Fire()
    {
        Vector3 secondPoint;
        if (!bFiring)
            lineRenderer.SetVertexCount(2);

        RaycastHit hit;
        if (Physics.RaycastAll(transform.position, transform.forward, distance)
            .WithMin(h => h.distance, h => !ShipIdentification.IsThisShip(h.transform, driver.ship), out hit))
        {
            var ship = hit.transform.GetComponent<ShipController>();
            if (ship != null)
                ship.AddEffect(new FreezeEffect());
            secondPoint = hit.point;
        }
        else
            secondPoint = firePoint.position + firePoint.forward * distance;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, secondPoint);
        bFiring = true;
    }
    protected override void SlowUpdate()
    {
        base.SlowUpdate();
        if(bFiring && !bfire)
        {
            lineRenderer.SetVertexCount(0);
            bFiring = false;
        }
    }
    protected override void Init(SystemComponent component, out float maxDistance)
    {
        maxDistance = ((FreezeRayComponent)component).distance;
    }
}

