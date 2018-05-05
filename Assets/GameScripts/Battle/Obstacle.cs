using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;
namespace Assets.GameScripts.Battle
{
    public class Obstacle : PBehaviour
    {
        public TempTransform transform { get; set; }
        public float radius { get { return _radius; }
            set {
                _radius = value;
                _sqrRadius = value * value;

                maneuverRadius = value * 2.5f;

                sqrRiskyRadius = value * 2f;
                sqrRiskyRadius *= sqrRiskyRadius;
            }
        }

        float _radius;
        float _sqrRadius;
        float sqrRiskyRadius;
        float maneuverRadius;
        public override void Start()
        {
            radius *= 1.25f;
            transform = GetComponent<TempTransform>();
            enabled = false;

        }

        public Vector3 GetMovePoint(Vector3 from, Vector3 to)
        {
            return GetMovePoint(GetXAxis(from, to));
        }
        public void GetMovePoint(ref NavResult ioR)
        {
            Vector3 point = transform.position - ioR.from;
            float sqrPointDistance = point.sqrMagnitude;
            float pointDistance = Mathf.Sqrt(sqrPointDistance);
            float radiusDivDistance = _radius / pointDistance;

            if (ioR.hit == HitType.Critical)
            {
                return;
            }
            else if (ioR.hit == HitType.Risky)
            {
                if (radiusDivDistance > ioR.radiusDivDistance)
                    return;
            }

            if (sqrPointDistance < sqrRiskyRadius)
            {
                ioR.radiusDivDistance = radiusDivDistance;
                ioR.pointDistance = pointDistance;
                ioR.hitDistance = 0;
                ioR.obstacle = this;
                if(pointDistance < _radius)
                {
                    ioR.hit = HitType.Critical;
                    ioR.movePoint = point * -(maneuverRadius / pointDistance);
                }
                else
                {
                    ioR.hit = HitType.Risky;
                    ioR.movePoint = GetXAxis(ioR.from, ioR.to) * maneuverRadius;
                }

                return;
            }
            
            float cosa = Vector3.Dot(point, ioR.rayAxis) / pointDistance;
            if (cosa > 1f) cosa = 1f;
            if (cosa < 0)
                return;
            float ySide = pointDistance * cosa;
            float sqrR = sqrPointDistance - ySide * ySide; //g^2 = k1^2 + k2^2
            if (sqrR < 0) sqrR *= -1;
            if (sqrR < sqrRiskyRadius)
            {
                var y = Mathf.Sqrt(sqrRiskyRadius - sqrR);
                var hitDistance = ySide - y;

                if ((hitDistance > ioR.rayDistance) || ((ioR.hit != HitType.None) && (hitDistance > ioR.hitDistance)))
                    return;

                ioR.radiusDivDistance = radiusDivDistance;
                ioR.pointDistance = pointDistance;
                ioR.hitDistance = hitDistance;
                ioR.hit = HitType.Safe;
                ioR.obstacle = this;
                ioR.movePoint = (sqrR != 0 ? (ioR.rayAxis * ySide - point) / Mathf.Sqrt(sqrR) : Vector3.Cross(Vector3.up, ioR.rayAxis)) * maneuverRadius;
            }
        }
        public Vector3 GetMovePoint(Vector3 xAxis)
        {
            return transform.position + xAxis * maneuverRadius;
        }

        Vector3 GetXAxis(Vector3 from, Vector3 to)
        {
            Vector3 d = to - from;
            Vector3 p = transform.position - from;

            if (p == Vector3.zero)
                return transform.position + Vector3.right * _radius;

            Vector3 up = Vector3.Cross(d, p);
            if (up == Vector3.zero)
                up = Vector3.up;

            Vector3 right = Vector3.Cross(p, up);

            right.Normalize();
            return right;
        }

        public override void FastUpdate()
        {
        }

        public override void InitializeUpdate()
        {
        }

        public override void SlowUpdate()
        {
        }
    }

    public struct NavResult
    {
        public Vector3 movePoint;

        public readonly Vector3 from;
        public readonly Vector3 to;
        public readonly Vector3 ray;
        public readonly Vector3 rayAxis;
        public readonly float rayDistance;

        public HitType hit;
        public Obstacle obstacle;
        public float hitDistance;
        public float pointDistance;
        public float radiusDivDistance;
        public NavResult(Vector3 from, Vector3 to)
        {
            movePoint = to;

            this.from = from;
            this.to = to;
            ray = to - from;
            rayDistance = ray.magnitude;
            rayAxis = ray / rayDistance;

            hit = HitType.None;
            obstacle = null;
            hitDistance = 0;
            pointDistance = 0;
            radiusDivDistance = 0;
        }
    }

    public enum HitType
    {
        None,
        Safe,
        Risky,
        Critical
    }
}
