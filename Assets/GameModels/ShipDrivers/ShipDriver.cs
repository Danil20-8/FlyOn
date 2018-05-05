using UnityEngine;
using System.Collections.Generic;
using Assets.GameModels.Battle;
using Assets.Global;
using Assets.Other;

public abstract class ShipDriver{

    const int max_fire_points = 3;

    public virtual void OnKill(ShipController ship) { }

    public ShipController ship { get; private set; }
    public Vector3 shipPosition { get { return ship.tempTransform.position; } }
    public int shipClass { get { return ship.shipClass; } }
    public ShipTeam shipTeam { get; private set; }

    public bool bFire { get; set; }
    public bool bRotate { get; set; }
    public Quaternion moveDirection { get; set; }
    public FirePoints firePoints { get; private set; }
    public List<ShipController> friendsAround { get; private set; }
    public List<ShipController> enemiesAround { get; private set; }
    FriendEnemyList shipsAround;

    float speedAcceleration;

    Vector3 moveForceResult;
    float rotationAngleResult;
    bool bmoving = false;

    public ShipDriver(ShipTeam team)
    {
        bFire = false;
        bRotate = false;
        moveDirection = Quaternion.identity;
        enemiesAround = new List<ShipController>();
        friendsAround = new List<ShipController>();
        shipsAround = new FriendEnemyList(team, friendsAround, enemiesAround);
        this.shipTeam = team;
        firePoints = new FirePoints(max_fire_points, this);
    }

    public void SetShip(ShipController ship)
    {
        if (this.ship != null)
            if(this.ship.alive)
                if(this is PlayerDriver)
                    throw new System.Exception("Ship's stil alive" + " " + this.ship.alive);
        this.ship = ship;

        ship.enabled = true;

        OnSetShip();
    }
    public void Accelerate(float multiply)
    {
        speedAcceleration += multiply;
    }
    public void Move(float speed)
    {
        moveForceResult += ship.tempTransform.forward * speed;
        bmoving = true;
    }
    public void Rotate(float speed)
    {
        rotationAngleResult += speed;
    }
    protected virtual void OnRotate(float speed)
    {

    }
    public virtual void InitializeUpdate()
    {
        moveForceResult = Vector3.zero;
        rotationAngleResult = 0;
    }
    public virtual void SlowUpdate()
    {
        if (bmoving)
        {
            ship.AddForce(moveForceResult * speedAcceleration);
            speedAcceleration = 1;
            bmoving = false;
        }
        if (bRotate)
        {
            ship.Rotate(Quaternion.RotateTowards(ship.tempTransform.rotation, moveDirection, rotationAngleResult));
            OnRotate(rotationAngleResult);
            bRotate = false;
        }
    }
    public virtual void FastUpdate()
    {
        BattleBehaviour.current.area.GetObjects(ship.tempTransform.position, 2000 * ship.shipClass, shipsAround);
    }
    protected virtual void OnSetShip()
    {

    }
    public virtual void Check()
    {
    }
    public virtual void OnDead()
    {
    }
    public virtual void OnDamaged(float damage)
    {
    }
}

public class FirePoints
{
    Vector3[] points;
    public Vector3 point { get { return points[0]; } set { points[0] = value; } }
    public int maxPoints { get { return points.Length; } }
    ShipDriver driver;

    public int count { get; set; }

    public FirePoints(int maxPoints, ShipDriver driver)
    {
        maxPoints = maxPoints < 1 ? 1 : maxPoints;

        points = new Vector3[maxPoints];

        this.driver = driver;
    }

    public void AddPoint(Vector3 point)
    {
        SetPoint(point, count++);
    }

    public void SetPoint(Vector3 point, int index)
    {
        points[index] = point;
    }

    public void Clear()
    {
        count = 0;
    }

    public IEnumerable<Point> GetFirePoints()
    {
        return new CustomEnumerable<Point>(GetFirePointsEnumerator());
    }

    public IEnumerator<Point> GetFirePointsEnumerator()
    {
        for (int i = 0; i < count; i++)
            yield return new Point(this, i);
    }

    Quaternion GetFireDirection(Vector3 from, int index)
    {
        Vector3 firePoint = points[index];
        ShipController ship = driver.ship;

        var point = firePoint - from;
        point.Normalize();
        var forward = ship.tempTransform.forward;
        var right = ship.tempTransform.right;
        var ups = ship.tempTransform.up;
        var cosa = Vector3.Dot(point, forward);
        var cosb = Vector3.Dot(point, right);
        var cost = Vector3.Dot(point, ups);
        Vector3 up;
        if (Mathf.Abs(cosb) < .08f && Mathf.Abs(cost) < .996f)
            up = cosa > 0 ? Vector3.Cross(point, right) : Vector3.Cross(right, point);
        else
            up = cosb < 0 ? Vector3.Cross(point, forward) : Vector3.Cross(forward, point);

        return Quaternion.LookRotation(point, up);
    }

    public struct Point
    {
        FirePoints points;
        int index;

        public Vector3 point { get { return points.points[index]; } }

        public Quaternion GetDirection(Vector3 from)
        {
            return points.GetFireDirection(from, index);
        }

        public Point(FirePoints points, int index)
        {
            this.points = points;
            this.index = index;
        }
    }
}