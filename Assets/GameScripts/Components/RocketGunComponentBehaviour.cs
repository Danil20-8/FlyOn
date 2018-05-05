using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;
using Assets.GameScripts.Components.Executors;
using Assets.GameModels;
using Assets.GameModels.Components;

namespace Assets.GameScripts.Components
{
    public class RocketGunComponentBehaviour : GunComponentBehaviour
    {
        float sqrMaxDistance;

        float _damage;
        float _deadTime;
        float _radius;
        float _speed;

        public void Fire(float damage, float distance, float speed, float radius)
        {
            _damage = damage;
            _deadTime = distance / speed;
            _speed = speed;
            _radius = radius;

            bfire = true;
        }

        protected override void Init(SystemComponent component, out float maxDistance)
        {
            maxDistance = ((RocketGunComponent)component).distance;
        }

        protected override void Fire()
        {
            RocketShot shot;
            shot.damage = _damage;
            shot.radius = _radius;
            shot.master = driver.ship;

            BattleBehaviour.current.shoting.GetSystem<RocketShots>().Fire(shot, shotPoint, shotPointTransform.forward, _speed, _deadTime, driver.ship);

            if (driver is PlayerDriver)
                BattleBehaviour.current.audioPlayer.Play("RocketShotAudio", shotPoint);
        }
    }
}

public struct RocketShot: IShot
{
    public float damage;
    public float radius;
    public ShipController master;

    public void Hit(RaycastHit hit)
    {
        if(damage > .5f)
            Explosion.Detonate(hit.point, Quaternion.identity, "Sphere2");
        else
            Explosion.Detonate(hit.point, Quaternion.identity, "Sphere1");

        var hits = Physics.OverlapSphere(hit.point, radius);

        foreach (var h in hits)
        {
            var c = h.GetComponentInParent<CombatBehaviour>();
            if (c != null)
            {
                c.SetDamage(damage, master);
            }
        }
        if (BattleBehaviour.current.audioPlayer.IsListener(hit.transform.root))
        {
            BattleBehaviour.current.audioPlayer.Play("HitAudio2", hit.point);
        }
    }

}