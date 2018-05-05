using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameModels.Components
{
    public class GunComponent : SystemComponent
    {
        [ComponentInfo("Damage")]
        public float damage { get; private set; }
        [ComponentInfo("Distance")]
        public float distance { get; private set; }
        [ComponentInfo("ShotSpeed")]
        public float speed { get; private set; }
        [ComponentInfo("GunRotation")]
        public float rotationSpeed { get; private set; }

        public GunComponent(int shipClass): base(shipClass)
        {
            damage = 0.2f + 0.2f * shipClass;
            distance = 2000 + 433 * shipClass;
            speed = 3000 + 225 * shipClass;
            rotationSpeed = 50f - 2.7f * shipClass;
            name = "Gun";
        }
        void Direct(PlasmaGunComponentBehaviour master, float rate, float timeSpend)
        {
            master.Direct(rate * 45f * timeSpend);
        }
        void Fire(PlasmaGunComponentBehaviour master, float rate)
        {
            master.Fire(rate * damage, distance * rate, speed * rate);
        }

        protected override ComponentActivity[] GetActivities(ShipComponentBehaviour shipComponent)
        {
            PlasmaGunComponentBehaviour gun = (PlasmaGunComponentBehaviour)shipComponent;
            return new ComponentActivity[] {
                new ContinuousComponentActivity<PlasmaGunComponentBehaviour>(gun, gun.IsDirecting, Direct),
                new TimerComponentActivity<PlasmaGunComponentBehaviour>(gun, .2f + .2f * shipClass, gun.IsFiring, Fire)
            };
        }
    }
}
