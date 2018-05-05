using UnityEngine;
using Assets.GameModels;
using System;
using System.Collections.Generic;
using System.Collections;

public abstract class GunComponentBehaviour : DirectableComponentBehaviour
{
    [SerializeField]
    protected Transform shotPointTransform;
    protected Vector3 shotPoint;

    float maxDistance;

    protected bool bfire = false;
    bool isFiring = false;

    protected override void OnSetLink(SystemComponent component)
    {
        Init(component, out maxDistance);
        maxDistance *= maxDistance;
    }
    protected abstract void Init(SystemComponent component, out float maxDistance);
    public bool IsFiring()
    {
        return isFiring;
    }
    protected override void InitializeUpdate()
    {
        base.InitializeUpdate();
        shotPoint = shotPointTransform.position;
    }

    protected override void SlowUpdate()
    {
        base.SlowUpdate();
        if (bfire)
        {
            Fire();

            bfire = false;
        }
    }
    protected abstract void Fire();

    protected override IEnumerator<Quaternion> GetDirections()
    {
        return new Rotations(this);
    }

    struct Rotations : IEnumerator<Quaternion>
    {
        GunComponentBehaviour gun;
        IEnumerator<FirePoints.Point> point;
        Quaternion rotation;
        bool end;

        public Rotations(GunComponentBehaviour gun)
        {
            this.gun = gun;
            point = gun.driver.firePoints.GetFirePointsEnumerator();
            rotation = Quaternion.identity;
            end = false;
        }

        Quaternion IEnumerator<Quaternion>.Current { get { return rotation; } }
        public FirePoints.Point Current { get { return point.Current; } }

        object IEnumerator.Current{ get { return Current; } }

        public void Dispose()
        {
            if (end)
            {
                gun.isFiring = false;
            }
            else if (Quaternion.Angle(gun.tempTransform.rotation, rotation) < 15)
            {
                gun.isFiring = gun.driver.bFire;
            }
        }

        public bool MoveNext()
        {
            while (point.MoveNext())
                if ((point.Current.point - gun.shotPoint).sqrMagnitude <= gun.maxDistance)
                {
                    rotation = point.Current.GetDirection(gun.tempTransform.position);
                    return true;
                }
            end = true;
            return false;
        }

        public void Reset()
        {
            point.Reset();
        }
    }
}