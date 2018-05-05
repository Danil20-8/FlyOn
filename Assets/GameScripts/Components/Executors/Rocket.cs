using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;

namespace Assets.GameScripts.Components.Executors
{
    [RequireComponent(typeof(Rigidbody))]
    public class Rocket: ShotBehaviour
    {
        float _damage;
        float _radius;
        ShipController _master;

        bool alive;

        void Start()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.AddComponent<CombatBehaviour>().SetOnDamage(OnDamage);
        }
        void OnDamage(float damage, ShipController offender)
        {
            Boom();
            Destroy();
        }
        void Boom()
        {
            if (!alive)
                return;
            alive = false;

            Explosion.Detonate(transform.position, Quaternion.identity, "Sphere");

            var hits = Physics.OverlapSphere(transform.position, _radius);

            foreach(var h in hits)
            {
                var c = h.GetComponentInParent<CombatBehaviour>();
                if(c != null)
                {
                    c.SetDamage(_damage, _master);
                }
            }
        }
        public static void Go(Transform start, float damage, float radius, float speed, float lifeTime, Transform friend, ShipController master)
        {
            var r = BattleBehaviour.current.pool.Get<Rocket>(@"Prefabs\Rocket", start.position, start.rotation, null);
            r._damage = damage;
            r._radius = radius;
            r._master = master;
            r.alive = true;
            r.AddShot(speed, lifeTime, friend);
        }

        protected override void HitSomething(RaycastHit hit)
        {
            Boom();
        }

        protected override void Destroy()
        {
            RemoveShot();
            BattleBehaviour.current.pool.Put(@"Prefabs\Rocket", this);
        }
    }
}
