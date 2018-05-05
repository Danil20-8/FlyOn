using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameScripts.Components;
namespace Assets.GameModels.Components
{
    public class RocketGunComponent: SystemComponent
    {
        float damage;
        float radius;
        float speed;
        float _distance;
        public float distance { get { return _distance; } }

        public RocketGunComponent(int shipClass): base(shipClass)
        {
            name = "RocketGun";
            speed = 2400 + 75 * shipClass;
            radius = 5 + 7 * shipClass;
            damage = .04f + .04f * shipClass;
            _distance = 1500 + 250 * shipClass;
        }

        void Direct(RocketGunComponentBehaviour master, float rate, float timeSpend)
        {
            master.Direct(rate * 45f * timeSpend);
        }

        void Fire(RocketGunComponentBehaviour gun, float rate)
        {
            if(rate >= 0.5f)
                gun.Fire(damage, _distance, speed, radius);
        }
        
        protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            RocketGunComponentBehaviour gun = (RocketGunComponentBehaviour)shipComponent;
            return new ComponentActivity[]
            {
                new ContinuousComponentActivity<RocketGunComponentBehaviour>(gun, gun.IsDirecting, Direct),
                new TimerComponentActivity<RocketGunComponentBehaviour>(gun, .3f + .3f * shipClass, gun.IsFiring, Fire)
            };
        }

    }
}
