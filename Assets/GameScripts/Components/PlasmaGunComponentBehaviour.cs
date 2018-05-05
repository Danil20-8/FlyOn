using System;
using Assets.GameModels;
using Assets.GameModels.Components;
using UnityEngine;

public class PlasmaGunComponentBehaviour : GunComponentBehaviour
{
    float damage;
    float lifeTime;
    float speed;

    int shotType;

    public void Fire(float damage, float distance, float speed)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = distance / speed;
        this.bfire = true;
    }

    protected override void Fire()
    {
        PlasmaShot shot;
        shot.damage = damage;
        shot.master = driver.ship;
        shot.rotation = shotPointTransform.rotation;

        BattleBehaviour.current.shoting.GetSystem<BlasterShots>().Fire(shot, shotPoint, shotPointTransform.forward, speed, lifeTime, driver.ship);
        if (driver is PlayerDriver)
            BattleBehaviour.current.audioPlayer.Play("BlasterShot" + shotType, shotPoint);
    }

    protected override void Init(SystemComponent component, out float maxDistance)
    {
        GunComponent gun = (GunComponent) component;
        maxDistance = gun.distance;
        shotType = gun.shipClass;
    }
}

public struct PlasmaShot : IShot
{
    public float damage;
    public ShipController master;
    public Quaternion rotation;

    public void Hit(RaycastHit hit)
    {
        Explosion e;
        rotation.w *= -1;

        var hitTransform = hit.collider.transform;

        if (damage < .3f)
            e = Explosion.Detonate(hit.point, rotation, "Little", hitTransform);
        else
            e = Explosion.Detonate(hit.point, rotation, "Big", hitTransform);

        var stricken = hitTransform.GetComponentInParent<CombatBehaviour>();
        if (stricken != null)
            stricken.SetDamage(damage, master);

        if (BattleBehaviour.current.audioPlayer.IsListener(hit.transform.root))
        {
            BattleBehaviour.current.audioPlayer.Play("HitAudio", hit.point);
        }
    }
}