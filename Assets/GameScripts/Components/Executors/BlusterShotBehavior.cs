using UnityEngine;
using System.Collections.Generic;
using MyLib.Algoriphms;
using Assets.Other.Special;
using Assets.Other;
using System.Linq;
[RequireComponent(typeof(LineRenderer))]
public class BlusterShotBehavior : ShotBehaviour {

    static Stack<BlusterShotBehavior> free = new Stack<BlusterShotBehavior>();
    public static void ClearPool() { free.Clear();}
    LineRenderer line;

    float damage = 0;
    ShipController master;

	// Use this for initialization
	void Awake () {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        gameObject.SetActive(false);
    }

    protected override void Destroy()
    {
        RemoveShot();
        gameObject.SetActive(false);
        free.Push(this);
    }
    public static void Go(BlusterShotBehavior bluster, Transform start, float damage, float speed, float lifeTime, Transform friend, ShipController master)
    {
        GetShot(start, bluster).Go(damage, speed, lifeTime, friend, master);
    }
    void Go(float damage, float speed, float lifeTime, Transform friend, ShipController master)
    {
        this.master = master;
        this.damage = damage;
        AddShot(speed, lifeTime, friend);
        gameObject.SetActive(true);
    }
    static BlusterShotBehavior GetShot(Transform transform, BlusterShotBehavior bluster)
    {
        if(free.Count > 0)
        {
            var shot = free.Pop();
            shot.transform.position = transform.position;
            shot.transform.rotation = transform.rotation;
            return shot;
        }
        else
            return (BlusterShotBehavior)Instantiate(bluster, transform.position, transform.rotation);
    }
    protected override void EndUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, _back);
    }
    protected override void HitSomething(RaycastHit hit)
    {
        /*if (hit.transform.GetComponent<ShieldPlane>() != null)
        {
            Destroy();
            return;
        }*/
        Explosion e;
        if(damage < .3f)
            e = Explosion.Detonate(hit.point, Quaternion.Inverse(transform.rotation), "Little", hit.collider.transform);
        else
            e = Explosion.Detonate(hit.point, Quaternion.Inverse(transform.rotation), "Big", hit.collider.transform);

        var hitTransform = hit.collider.transform;
        var stricken = hitTransform.GetComponentInParent<CombatBehaviour>();
        if (stricken != null)
            stricken.SetDamage(damage, master);
        Destroy();
    }
    /*void OnTriggerEnter(Collider col)
    {
        if (!col.transform.IsOne(friend))
        {
            var op = oldPosition;
            var dir = transform.position - op;
            var ray = new Ray(op, dir);
            RaycastHit hit;
            col.Raycast(ray, out hit, dir.magnitude);
            GameObject e;
            if (hit.collider == null)
                e = (GameObject)Instantiate(Resources.Load(@"Prefabs\Explosion"), transform.position, Quaternion.Inverse(transform.rotation));
            else
                e = (GameObject)Instantiate(Resources.Load(@"Prefabs\Explosion"), hit.point + hit.normal, Quaternion.LookRotation(hit.normal));
            e.transform.SetParent(col.transform);

            var stricken = col.GetComponent<CombatBehaviour>();
            if (stricken != null)
                SetDamage(stricken);
            Destroy();
        }
    }*/
}
